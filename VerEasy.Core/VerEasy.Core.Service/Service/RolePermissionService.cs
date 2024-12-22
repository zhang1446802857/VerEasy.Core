using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Service.Base;

namespace VerEasy.Core.Service.Service
{
    public class RolePermissionService(IBaseRepository<RolePermission> baseRepo) : BaseService<RolePermission>(baseRepo), IRolePermissionService
    {
        public async Task<bool> AssignPermissionsAsync(ParamDto.RolePermissionParam param)
        {
            var result = await Query(x => x.RoleId == param.RoleId && !x.IsDeleted);

            // 获取现有的权限Id集合
            var existingIds = result.Select(x => x.PermissionId).ToHashSet();

            // 找出需要添加的权限Id（那些现有记录没有的）
            var permissionsToAdd = param.PermissionIds
                .Where(permissionId => !existingIds.Contains(permissionId))
                .Select(permissionId => new RolePermission
                {
                    RoleId = param.RoleId,
                    PermissionId = permissionId
                }).ToList();

            // 找出需要删除的权限Id（那些现有记录不再需要的）
            var permissionsToDelete = result
                .Where(x => !param.PermissionIds.Contains(x.PermissionId))
                .ToList();

            // 删除多余的记录
            await Delete(permissionsToDelete);

            // 添加缺失的记录
            if (permissionsToAdd.Count != 0)
            {
                await Add(permissionsToAdd);
            }

            return true;
        }

        public async Task<long[]> GetPermissionsIds(long[] id)
        {
            var result = await Query(x => !x.IsDeleted && id.Contains(x.RoleId));
            return result.Select(x => x.PermissionId).Distinct().ToArray();
        }

        public async Task<string[]> GetRolePermissionsAsync(long roleId)
        {
            var result = await Query(x => x.RoleId == roleId && !x.IsDeleted);
            return result.Select(x => x.PermissionId.ToString()).ToArray();
        }
    }
}