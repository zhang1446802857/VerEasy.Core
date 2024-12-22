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

            // ��ȡ���е�Ȩ��Id����
            var existingIds = result.Select(x => x.PermissionId).ToHashSet();

            // �ҳ���Ҫ��ӵ�Ȩ��Id����Щ���м�¼û�еģ�
            var permissionsToAdd = param.PermissionIds
                .Where(permissionId => !existingIds.Contains(permissionId))
                .Select(permissionId => new RolePermission
                {
                    RoleId = param.RoleId,
                    PermissionId = permissionId
                }).ToList();

            // �ҳ���Ҫɾ����Ȩ��Id����Щ���м�¼������Ҫ�ģ�
            var permissionsToDelete = result
                .Where(x => !param.PermissionIds.Contains(x.PermissionId))
                .ToList();

            // ɾ������ļ�¼
            await Delete(permissionsToDelete);

            // ���ȱʧ�ļ�¼
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