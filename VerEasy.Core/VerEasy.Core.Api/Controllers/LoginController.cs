using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Extensions.Authorization;
using static VerEasy.Core.Models.Dtos.ParamDto;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IUserService userService, IUserRoleService userRoleService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IUserRoleService _userRoleService = userRoleService;

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost("LoginJwt")]
        public async Task<MessageModel<string>> LoginJwt(LoginParam param)
        {
            var result = await _userService.LoginUserAsync(param);
            if (result.Success)
            {
                var user = result.Response;
                //获取角色信息
                var roles = await _userRoleService.Query(x => x.UserId == user.Id && !x.IsDeleted);
                var roleIds = string.Join(",", roles.Select(x => x.RoleId));
                var token = JwtHelper.IssueJwtToken(new JwtHelper.TokenModelJwt
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Role = roleIds,
                });
                return MessageModel<string>.Ok("登录成功", token);
            }
            return result.ConvertTo<User, string>();
        }
    }
}