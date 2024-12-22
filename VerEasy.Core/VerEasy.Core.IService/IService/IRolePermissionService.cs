using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ParamDto;

namespace VerEasy.Core.IService.IService
{
    public interface IRolePermissionService : IBaseService<RolePermission>
    {
        /// <summary>
        /// 分配权限
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<bool> AssignPermissionsAsync(RolePermissionParam param);

        /// <summary>
        /// 获取角色对应的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<string[]> GetRolePermissionsAsync(long roleId);

        /// <summary>
        /// 根据角色ID获取权限ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<long[]> GetPermissionsIds(long[] id);
    }
}