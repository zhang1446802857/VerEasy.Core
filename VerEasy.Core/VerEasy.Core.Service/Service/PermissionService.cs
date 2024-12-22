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
            results = [.. results.OrderBy(x => x.SuperiorRelation.Length)];  // �����Ż�

            // �洢���ڵ���ӽڵ��ϵ���ֵ�
            var resultDto = new List<PermissionResult>();
            var lookup = new Dictionary<string, PermissionResult>();

            foreach (var item in results)
            {
                var itemNode = new PermissionResult
                {
                    Children = [], // ��ʼ���ӽڵ�
                    Id = item.Id.ToString(),
                    CreateTime = item.CreateTime.ToShortDateString(),
                    Enable = item.Enable,
                    Name = item.Type == Models.Enums.PermissionType.Menu ? "����" : item.Name,
                    Title = item.Title,
                    Component = item.Component,
                    SuperiorRelation = item.SuperiorRelation,
                    UpdateTime = item.UpdateTime.ToShortDateString(),
                    Type = item.Type,
                    Description = item.Description,
                    Path = item.Type == Models.Enums.PermissionType.Menu ? "����" : "/" + item.Path
                };

                // ��ȡ����Id��ͨ���ֵ���в���
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
                            Name = item.Type == Models.Enums.PermissionType.Menu ? "����" : item.Name,
                            SuperiorRelation = item.SuperiorRelation,
                            UpdateTime = item.UpdateTime.ToShortDateString(),
                            Type = departParent.Type,
                            Path = item.Type == Models.Enums.PermissionType.Menu ? "����" : "/" + departParent.Path,
                            Component = departParent.Component,
                            Title = departParent.Title,
                            Description = departParent.Description,
                        };
                        // ������ڵ㲻�����ֵ��У��򴴽�����ӵ��ֵ�
                        lookup[parentIdStr] = value;
                    }

                    // ��ȡ���ڵ㲢����ӽڵ�
                    var parentNode = value;
                    parentNode.Children.Add(itemNode);
                }
                else
                {
                    // ���ڵ㣬ֱ����ӵ� resultDto
                    resultDto.Add(itemNode);
                }

                // ����ǰ�ڵ���ӵ��ֵ��У��Ա����ʹ��
                lookup[itemNode.Id] = itemNode;
            }

            return resultDto;
        }

        /// <summary>
        /// ��ȡ���и�������
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
            //�����
            var results = await Query(x => !x.IsDeleted);

            //���ݸ��ӹ�ϵ������,�Ӹ�����ʼ
            results = [.. results.OrderBy(x => x.SuperiorRelation.Length)];

            var rootDepartmentResult = new CascaderResult
            {
                Children = [],
                Label = "Root",
                Value = "0"
            };

            //���ؼ���
            var resultDto = new List<CascaderResult>();

            //�ֵ�
            var departmentsDic = new Dictionary<long, CascaderResult>();

            resultDto.Add(rootDepartmentResult);
            departmentsDic.Add(0, rootDepartmentResult);

            foreach (var item in results)
            {
                //�����ֵ䲻����,������ȥ
                if (!departmentsDic.TryGetValue(item.Id, out var itemNode))
                {
                    itemNode = new CascaderResult { Value = item.Id.ToString(), Label = item.Title, Children = [] };
                    departmentsDic[item.Id] = itemNode;
                }

                //��ȡ����
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
                return MessageModel<bool>.Ok("�޸ĳɹ�");
            }
            return MessageModel<bool>.Fail("δ��ѯ��ƥ������,��ˢ�³���");
        }

        public async Task<List<VueRouterResult>> GetVueRouterAsync(long[] ids, bool all)
        {
            var results = new List<Permission>();
            //���ݽ�ɫid��ѯ���е�Ȩ����Ϣ
            if (!all)
            {
                results = await Query(x => ids.Contains(x.Id));
            }
            else
            {
                results = await Query(x => !x.IsDeleted);
            }
            results = [.. results.OrderBy(x => x.SuperiorRelation.Length)];  // �����Ż�

            // �洢���ڵ���ӽڵ��ϵ���ֵ�
            var resultDto = new List<VueRouterResult>();
            var lookup = new Dictionary<string, VueRouterResult>();

            foreach (var item in results)
            {
                var itemNode = new VueRouterResult
                {
                    Children = [], // ��ʼ���ӽڵ�
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

                // ��ȡ����Id��ͨ���ֵ���в���
                var parentId = item.SuperiorRelation.Split(',').Select(long.Parse).Last();
                string parentIdStr = parentId.ToString();

                if (parentId != 0)
                {
                    if (!lookup.TryGetValue(parentIdStr, out VueRouterResult? value))
                    {
                        var departParent = results.First(x => x.Id == parentId);
                        value = new VueRouterResult
                        {
                            Children = [], // ��ʼ���ӽڵ�
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
                        // ������ڵ㲻�����ֵ��У��򴴽�����ӵ��ֵ�
                        lookup[parentIdStr] = value;
                    }

                    // ��ȡ���ڵ㲢����ӽڵ�
                    var parentNode = value;
                    parentNode.Children.Add(itemNode);
                }
                else
                {
                    // ���ڵ㣬ֱ����ӵ� resultDto
                    resultDto.Add(itemNode);
                }

                // ����ǰ�ڵ���ӵ��ֵ��У��Ա����ʹ��
                lookup[item.Id.ToString()] = itemNode;
            }

            return resultDto;
        }
    }
}