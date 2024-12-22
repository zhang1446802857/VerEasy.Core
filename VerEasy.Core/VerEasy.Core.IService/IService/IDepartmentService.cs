using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.IService.IService
{
    public interface IDepartmentService : IBaseService<Department>
    {
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<bool> AddDepartmentAsync(DepartmentParam param);

        /// <summary>
        /// ��ѯ����-Ƕ�׸�ʽ
        /// </summary>
        /// <returns></returns>
        Task<List<CascaderResult>> QueryDepartmentsByCascaderAsync();

        /// <summary>
        /// ��ѯ������Ϣ
        /// </summary>
        /// <returns></returns>
        Task<List<DepartmentResult>> QueryDepartmentResultsAsync();

        /// <summary>
        /// �޸Ĳ�����Ϣ
        /// </summary>
        /// <returns></returns>
        Task<bool> EditDepartmentAsync(DepartmentParam param);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MessageModel<bool>> DeletedDepartmentByIdAsync(string id);
    }
}