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
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<bool> AddRoleAsync(RoleParam role);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteRoleAsync(long id);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<MessageModel<bool>> EditRoleAsync(RoleParam role);

        /// <summary>
        /// 查询角色列表
        /// </summary>
        /// <returns></returns>
        Task<List<RoleResult>> QueryRoleAsync();
    }
}