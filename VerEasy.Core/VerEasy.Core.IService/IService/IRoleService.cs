using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.IService.IService
{
    public interface IRoleService : IBaseService<Role>
    {
        /// <summary>
        /// ��ӽ�ɫ
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> AddRoleAsync(RoleParam role);

        /// <summary>
        /// ɾ����ɫ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteRoleAsync(long id);

        /// <summary>
        /// �޸Ľ�ɫ
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<MessageModel<bool>> EditRoleAsync(RoleParam role);

        /// <summary>
        /// ��ѯ��ɫ�б�
        /// </summary>
        /// <returns></returns>
        Task<List<RoleResult>> QueryRoleAsync();
    }
}