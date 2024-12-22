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
        /// 添加菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        Task<bool> AddMenuAsync(PermissionParam menu);

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        Task<MessageModel<bool>> EditMenuAsync(PermissionParam menu);

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionResult>> QueryMenusAsync();

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteMenusAsync(long id);

        /// <summary>
        /// 查询层级菜单
        /// </summary>
        /// <returns></returns>
        Task<List<CascaderResult>> QueryMenusByCascaderAsync();

        /// <summary>
        /// 根据权限生成对应的前端路由配置
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<VueRouterResult>> GetVueRouterAsync(long[] ids, bool all);
    }
}