using AutoMapper;
using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Service.Base;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Service.Service
{
    public class PermissionService(IBaseRepository<Permission> baseRepo, IMapper mapper) : BaseService<Permission>(baseRepo), IPermissionService
    {
        public async Task<bool> AddMenuAsync(ParamDto.PermissionParam menu)
        {
            var result = mapper.Map<Permission>(menu);
            if (result.Type == Models.Enums.PermissionType.Menu)
            {
                result.Path = result.Name = Guid.NewGuid().ToString("N");
            }
            if (await Add(result) > 0)
            {
                return true;
            };
            return false;
        }

        public async Task<bool> DeleteMenusAsync(long id)
        {
            return await DeleteById(id);
        }

        public async Task<List<PermissionResult>> QueryMenusAsync()
        {
            var results = await Query(x => !x.IsDeleted);
            results = [.. results.OrderBy(x => x.SuperiorRelation.Length)];  // 排序优化

            // 存储父节点和子节点关系的字典
            var resultDto = new List<PermissionResult>();
            var lookup = new Dictionary<string, PermissionResult>();

            foreach (var item in results)
            {
                var itemNode = new PermissionResult
                {
                    Children = [], // 初始化子节点
                    Id = item.Id.ToString(),
                    CreateTime = item.CreateTime.ToShortDateString(),
                    Enable = item.Enable,
                    Name = item.Type == Models.Enums.PermissionType.Menu ? "——" : item.Name,
                    Title = item.Title,
                    Component = item.Component,
                    SuperiorRelation = item.SuperiorRelation,
                    UpdateTime = item.UpdateTime.ToShortDateString(),
                    Type = item.Type,
                    Description = item.Description,
                    Path = item.Type == Models.Enums.PermissionType.Menu ? "——" : "/" + item.Path
                };

                // 获取父级Id并通过字典进行查找
                var parentId = item.SuperiorRelation.Split(',').Select(long.Parse).Last();
                string parentIdStr = parentId.ToString();

                if (parentId != 0)
                {
                    if (!lookup.TryGetValue(parentIdStr, out PermissionResult? value))
                    {
                        var departParent = results.First(x => x.Id == parentId);
                        value = new PermissionResult
                        {
                            Children = [],
                            Id = parentIdStr,
                            CreateTime = item.CreateTime.ToShortDateString(),
                            Enable = item.Enable,
                            Name = item.Type == Models.Enums.PermissionType.Menu ? "——" : item.Name,
                            SuperiorRelation = item.SuperiorRelation,
                            UpdateTime = item.UpdateTime.ToShortDateString(),
                            Type = departParent.Type,
                            Path = item.Type == Models.Enums.PermissionType.Menu ? "——" : "/" + departParent.Path,
                            Component = departParent.Component,
                            Title = departParent.Title,
                            Description = departParent.Description,
                        };
                        // 如果父节点不存在字典中，则创建并添加到字典
                        lookup[parentIdStr] = value;
                    }

                    // 获取父节点并添加子节点
                    var parentNode = value;
                    parentNode.Children.Add(itemNode);
                }
                else
                {
                    // 根节点，直接添加到 resultDto
                    resultDto.Add(itemNode);
                }

                // 将当前节点添加到字典中，以便后续使用
                lookup[itemNode.Id] = itemNode;
            }

            return resultDto;
        }

        /// <summary>
        /// 获取所有父级名称
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentIds"></param>
        /// <returns></returns>
        public static string GetParentName(List<Permission> list, string parentIds)
        {
            var Ids = parentIds.Split(',');
            var parentNames = new List<string>();
            foreach (var id in Ids)
            {
                parentNames.Add(list.First(x => x.Id.ToString() == id).Name);
            }

            return string.Join(">", parentNames);
        }

        public async Task<List<CascaderResult>> QueryMenusByCascaderAsync()
        {
            //结果集
            var results = await Query(x => !x.IsDeleted);

            //根据父子关系列排序,从父级开始
            results = [.. results.OrderBy(x => x.SuperiorRelation.Length)];

            var rootDepartmentResult = new CascaderResult
            {
                Children = [],
                Label = "Root",
                Value = "0"
            };

            //返回集合
            var resultDto = new List<CascaderResult>();

            //字典
            var departmentsDic = new Dictionary<long, CascaderResult>();

            resultDto.Add(rootDepartmentResult);
            departmentsDic.Add(0, rootDepartmentResult);

            foreach (var item in results)
            {
                //数据字典不存在,新增进去
                if (!departmentsDic.TryGetValue(item.Id, out var itemNode))
                {
                    itemNode = new CascaderResult { Value = item.Id.ToString(), Label = item.Title, Children = [] };
                    departmentsDic[item.Id] = itemNode;
                }

                //获取父级
                var parentId = item.SuperiorRelation.Split(',').Select(long.Parse).Last();

                if (departmentsDic.TryGetValue(parentId, out var parentNode))
                {
                    parentNode.Children.Add(itemNode);
                }
                else
                {
                    var parentData = results.FirstOrDefault(x => x.Id == parentId);
                    if (parentData != null)
                    {
                        var newParentNode = new CascaderResult
                        {
                            Value = parentId.ToString(),
                            Label = parentData.Name,
                            Children = [itemNode]
                        };
                        departmentsDic[parentId] = newParentNode;
                        resultDto.Add(newParentNode);
                    }
                    else
                    {
                        resultDto.Add(itemNode);
                    }
                }
            }
            return resultDto;
        }

        public async Task<MessageModel<bool>> EditMenuAsync(ParamDto.PermissionParam menu)
        {
            var result = await QueryById(menu.Id);
            if (result != null)
            {
                if (!string.IsNullOrEmpty(menu.Component))
                {
                    result.Component = menu.Component;
                }
                if (!string.IsNullOrEmpty(menu.Name))
                {
                    result.Name = menu.Name;
                }
                if (!string.IsNullOrEmpty(menu.Path))
                {
                    result.Path = menu.Path;
                }
                if (!string.IsNullOrEmpty(menu.SuperiorRelation))
                {
                    result.SuperiorRelation = menu.SuperiorRelation;
                }
                if (!string.IsNullOrEmpty(menu.Description))
                {
                    result.Description = menu.Description;
                }
                result.Enable = menu.Enable;
                result.Type = menu.Type;
                await Update(result);
                return MessageModel<bool>.Ok("修改成功");
            }
            return MessageModel<bool>.Fail("未查询到匹配数据,请刷新尝试");
        }

        public async Task<List<VueRouterResult>> GetVueRouterAsync(long[] ids, bool all)
        {
            var results = new List<Permission>();
            //根据角色id查询所有的权限信息
            if (!all)
            {
                results = await Query(x => ids.Contains(x.Id));
            }
            else
            {
                results = await Query(x => !x.IsDeleted);
            }
            results = [.. results.OrderBy(x => x.SuperiorRelation.Length)];  // 排序优化

            // 存储父节点和子节点关系的字典
            var resultDto = new List<VueRouterResult>();
            var lookup = new Dictionary<string, VueRouterResult>();

            foreach (var item in results)
            {
                var itemNode = new VueRouterResult
                {
                    Children = [], // 初始化子节点
                    Name = item.Name,
                    Path = item.Type == Models.Enums.PermissionType.Menu ? "/M" + item.Path : "/" + item.Path,
                    Component = item.Component,
                    Meta = new Meta
                    {
                        Icon = item.Icon,
                        Loading = item.Loading,
                        Title = item.Title,
                    }
                };

                // 获取父级Id并通过字典进行查找
                var parentId = item.SuperiorRelation.Split(',').Select(long.Parse).Last();
                string parentIdStr = parentId.ToString();

                if (parentId != 0)
                {
                    if (!lookup.TryGetValue(parentIdStr, out VueRouterResult? value))
                    {
                        var departParent = results.First(x => x.Id == parentId);
                        value = new VueRouterResult
                        {
                            Children = [], // 初始化子节点
                            Name = item.Name,
                            Path = item.Path,
                            Component = item.Component,
                            Meta = new Meta
                            {
                                Icon = item.Icon,
                                Loading = item.Loading,
                                Title = item.Name,
                            }
                        };
                        // 如果父节点不存在字典中，则创建并添加到字典
                        lookup[parentIdStr] = value;
                    }

                    // 获取父节点并添加子节点
                    var parentNode = value;
                    parentNode.Children.Add(itemNode);
                }
                else
                {
                    // 根节点，直接添加到 resultDto
                    resultDto.Add(itemNode);
                }

                // 将当前节点添加到字典中，以便后续使用
                lookup[item.Id.ToString()] = itemNode;
            }

            return resultDto;
        }
    }
}