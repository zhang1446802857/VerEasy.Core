using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Extensions.ServiceExtensions.HttpContext;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController(IRolePermissionService service, IPermissionService permissionService, IUser user) : ControllerBase
    {
        public readonly IRolePermissionService _service = service;
        private readonly IPermissionService _permissionService = permissionService;
        private readonly IUser user = user;

        [HttpGet("QueryAll")]
        [Authorize]
        public async Task<MessageModel<List<RolePermission>>> QueryAll()
        {
            var a = user.UserName;
            return MessageModel<List<RolePermission>>.Ok(await _service.Query());
        }

        [HttpPost("GetVueRouterResult")]
        public async Task<MessageModel<List<VueRouterResult>>> GetVueRouterResult(RouterParam param)
        {
            //权限ID
            var permissionIds = await _service.GetPermissionsIds(param.Ids);
            var result = await _permissionService.GetVueRouterAsync(permissionIds, param.All);
            return MessageModel<List<VueRouterResult>>.Ok(result);
        }

        /// <summary>
        /// 分配权限
        /// </summary>
        /// <returns></returns>
        [HttpPost("AssignPermissionsAsync")]
        public async Task<MessageModel<bool>> AssignPermissionsAsync(RolePermissionParam param)
        {
            return MessageModel<bool>.Ok(await _service.AssignPermissionsAsync(param));
        }

        /// <summary>
        /// 获取角色对应的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("GetRolePermissionsAsync")]
        public async Task<MessageModel<string[]>> GetRolePermissionsAsync(long roleId)
        {
            return MessageModel<string[]>.Ok("OK", await _service.GetRolePermissionsAsync(roleId));
        }
    }
}