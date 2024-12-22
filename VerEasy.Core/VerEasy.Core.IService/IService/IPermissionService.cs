using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.IService.IService
{
    public interface IPermissionService : IBaseService<Permission>
    {
        /// <summary>
        /// ��Ӳ˵�
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        Task<bool> AddMenuAsync(PermissionParam menu);

        /// <summary>
        /// �޸Ĳ˵�
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        Task<MessageModel<bool>> EditMenuAsync(PermissionParam menu);

        /// <summary>
        /// ��ѯ�˵�
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionResult>> QueryMenusAsync();

        /// <summary>
        /// ����IDɾ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteMenusAsync(long id);

        /// <summary>
        /// ��ѯ�㼶�˵�
        /// </summary>
        /// <returns></returns>
        Task<List<CascaderResult>> QueryMenusByCascaderAsync();

        /// <summary>
        /// ����Ȩ�����ɶ�Ӧ��ǰ��·������
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<VueRouterResult>> GetVueRouterAsync(long[] ids, bool all);
    }
}