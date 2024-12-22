using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using VerEasy.Extensions.ServiceExtensions.HttpContext;

namespace VerEasy.Extensions.ServiceExtensions
{
    /// <summary>
    /// 注入HttpContex相关的接口,用于通过Token直接获取用户的某些信息
    /// </summary>
    public static class HttpContextUserSetup
    {
        public static void AddHttpContextUserSetup(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, HttpContextUser>();
        }
    }
}