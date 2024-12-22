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
        /// ����������Ϣ
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
            results = [.. results.OrderBy(x => x.SuperiorRelation.Length)];  // �����Ż�

            // �洢���ڵ���ӽڵ��ϵ���ֵ�
            var resultDto = new List<DepartmentResult>();
            var lookup = new Dictionary<string, DepartmentResult>();

            foreach (var item in results)
            {
                var itemNode = new DepartmentResult
                {
                    Children = [], // ��ʼ���ӽڵ�
                    Id = item.Id.ToString(),
                    CreateTime = item.CreateTime.ToShortDateString(),
                    Enable = item.Enable,
                    Name = item.Name,
                    SuperiorRelation = item.SuperiorRelation,
                    UpdateTime = item.UpdateTime.ToShortDateString()
                };

                // ��ȡ����Id��ͨ���ֵ���в���
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
                        // ������ڵ㲻�����ֵ��У��򴴽�����ӵ��ֵ�
                        lookup[parentIdStr] = value;
                    }

                    // ��ȡ���ڵ㲢����ӽڵ�
                    var parentNode = value;
                    itemNode.ParentName = GetParentName(results, item.SuperiorRelation);
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
            if (results.Count == 0)
            {
                resultDto.Add(rootDepartmentResult);
                departmentsDic.Add(0, rootDepartmentResult);
            }
            foreach (var item in results)
            {
                //�����ֵ䲻����,������ȥ
                if (!departmentsDic.TryGetValue(item.Id, out var itemNode))
                {
                    itemNode = new CascaderResult { Value = item.Id.ToString(), Label = item.Name, Children = [] };
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
            //�鿴�������õ��Ӽ�,���޷�ɾ��
            var result = await Query(x => !x.IsDeleted && x.SuperiorRelation.Contains(id));
            if (result.Count != 0)
            {
                return MessageModel<bool>.Fail("�ò������Ӳ��Ŵ���,�޷�ֱ��ɾ��");
            }
            return MessageModel<bool>.Ok(await DeleteById(id));
        }
    }
}