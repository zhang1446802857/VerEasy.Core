using Microsoft.AspNetCore.Http;
using VerEasy.Common.Helper;
using VerEasy.Common.Utils;

namespace VerEasy.Extensions.ServiceExtensions.HttpContext
{
    /// <summary>
    /// 通过httpcontext获取用户信息
    /// </summary>
    /// <param name="accessor"></param>
    public class HttpContextUser(IHttpContextAccessor accessor) : IUser
    {
        private readonly IHttpContextAccessor accessor = accessor;

        public string UserName => GetUserName();

        public long Id => GetUid();

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <returns></returns>
        private string GetUserName()
        {
            if (IsAuthenticated())
            {
                return accessor.HttpContext.User.Claims.First(x => x.Type == "name").Value;
            }
            return Appsettings.App("AppConfig:ApiName");
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            var token = accessor.HttpContext?.Request?.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
            //if (!string.IsNullOrEmpty(token))
            //{
            //    return token;
            //}
            return token;
        }

        /// <summary>
        /// 检验是否经过身份验证
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        /// <summary>
        /// 获取Uid
        /// </summary>
        /// <returns></returns>
        public long GetUid()
        {
            if (IsAuthenticated())
            {
                return accessor.HttpContext.User.Claims.First(x => x.Type == "jti").Value.ObjToLong();
            }
            return 0;
        }
    }
}