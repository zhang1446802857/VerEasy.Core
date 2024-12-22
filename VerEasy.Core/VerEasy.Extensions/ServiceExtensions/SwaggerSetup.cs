using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Runtime.InteropServices;
using VerEasy.Common.Helper;

namespace VerEasy.Extensions.ServiceExtensions
{
    /// <summary>
    /// swagger相关
    /// </summary>
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var basePath = AppContext.BaseDirectory;
            var apiName = Appsettings.App("Infos", "ApiName");
            services.AddSwaggerGen(x =>
            {
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    x.SwaggerDoc(version, new OpenApiInfo
                    {
                        Title = $"{apiName} 接口文档——{RuntimeInformation.FrameworkDescription}",
                        Description = $"{version} HTTP API {version}版本",
                        Version = version
                    });
                });
                var xmlPath = Path.Combine(basePath, "API.XML");
                x.IncludeXmlComments(xmlPath);

                x.OperationFilter<AddResponseHeadersFilter>();
                x.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                x.OperationFilter<SecurityRequirementsOperationFilter>();

                x.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Jwt授权",
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }
    }

    /// <summary>
    /// 自定义版本
    /// </summary>
    public enum ApiVersion
    {
        /// <summary>
        /// v1版本
        /// </summary>
        V1 = 1,

        /// <summary>
        /// v2版本
        /// </summary>
        V2 = 2,

        /// <summary>
        /// v3版本
        /// </summary>
        V3 = 3,
    }
}