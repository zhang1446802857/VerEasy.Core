using AutoMapper;
using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Service.Base;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Service.Service
{
    public class DepartmentService(IBaseRepository<Department> baseRepo, IMapper mapper) : BaseService<Department>(baseRepo), IDepartmentService
    {
        /// <summary>
        /// 新增部门信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<bool> AddDepartmentAsync(DepartmentParam param)
        {
            var newDepartment = mapper.Map<Department>(param);
            if (await Add(newDepartment) > 0)
            {
                return true;
            };
            return false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<DepartmentResult>> QueryDepartmentResultsAsync()
        {
            var results = await Query(x => !x.IsDeleted);
            results = [.. results.OrderBy(x => x.SuperiorRelation.Length)];  // 排序优化

            // 存储父节点和子节点关系的字典
            var resultDto = new List<DepartmentResult>();
            var lookup = new Dictionary<string, DepartmentResult>();

            foreach (var item in results)
            {
                var itemNode = new DepartmentResult
                {
                    Children = [], // 初始化子节点
                    Id = item.Id.ToString(),
                    CreateTime = item.CreateTime.ToShortDateString(),
                    Enable = item.Enable,
                    Name = item.Name,
                    SuperiorRelation = item.SuperiorRelation,
                    UpdateTime = item.UpdateTime.ToShortDateString()
                };

                // 获取父级Id并通过字典进行查找
                var parentId = item.SuperiorRelation.Split(',').Select(long.Parse).Last();
                string parentIdStr = parentId.ToString();

                if (parentId != 0)
                {
                    if (!lookup.TryGetValue(parentIdStr, out DepartmentResult? value))
                    {
                        var departParent = results.First(x => x.Id == parentId);
                        value = new DepartmentResult
                        {
                            Children = [],
                            Id = parentIdStr,
                            CreateTime = item.CreateTime.ToShortDateString(),
                            Enable = item.Enable,
                            Name = item.Name,
                            SuperiorRelation = item.SuperiorRelation,
                            UpdateTime = item.UpdateTime.ToShortDateString()
                        };
                        // 如果父节点不存在字典中，则创建并添加到字典
                        lookup[parentIdStr] = value;
                    }

                    // 获取父节点并添加子节点
                    var parentNode = value;
                    itemNode.ParentName = GetParentName(results, item.SuperiorRelation);
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
        public static string GetParentName(List<Department> list, string parentIds)
        {
            var Ids = parentIds.Split(',');
            var parentNames = new List<string>();
            foreach (var id in Ids)
            {
                parentNames.Add(list.First(x => x.Id.ToString() == id).Name);
            }

            return string.Join(">", parentNames);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<CascaderResult>> QueryDepartmentsByCascaderAsync()
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
            if (results.Count == 0)
            {
                resultDto.Add(rootDepartmentResult);
                departmentsDic.Add(0, rootDepartmentResult);
            }
            foreach (var item in results)
            {
                //数据字典不存在,新增进去
                if (!departmentsDic.TryGetValue(item.Id, out var itemNode))
                {
                    itemNode = new CascaderResult { Value = item.Id.ToString(), Label = item.Name, Children = [] };
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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> EditDepartmentAsync(DepartmentParam param)
        {
            if (await QueryById(param.Id) != null)
            {
                var newResult = mapper.Map<Department>(param);
                return await Update(newResult);
            }
            return false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MessageModel<bool>> DeletedDepartmentByIdAsync(string id)
        {
            //查看有无启用的子集,有无法删除
            var result = await Query(x => !x.IsDeleted && x.SuperiorRelation.Contains(id));
            if (result.Count != 0)
            {
                return MessageModel<bool>.Fail("该部门有子部门存在,无法直接删除");
            }
            return MessageModel<bool>.Ok(await DeleteById(id));
        }
    }
}