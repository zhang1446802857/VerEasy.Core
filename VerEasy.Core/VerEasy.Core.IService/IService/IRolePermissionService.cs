using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ParamDto;

namespace VerEasy.Core.IService.IService
{
    public interface IRolePermissionService : IBaseService<RolePermission>
    {
        /// <summary>
        /// ����Ȩ��
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<bool> AssignPermissionsAsync(RolePermissionParam param);

        /// <summary>
        /// ��ȡ��ɫ��Ӧ��Ȩ��
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<string[]> GetRolePermissionsAsync(long roleId);

        /// <summary>
        /// ���ݽ�ɫID��ȡȨ��ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<long[]> GetPermissionsIds(long[] id);
    }
}