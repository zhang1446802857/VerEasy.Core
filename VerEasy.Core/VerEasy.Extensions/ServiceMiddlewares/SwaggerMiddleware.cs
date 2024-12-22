using Microsoft.AspNetCore.Builder;
using VerEasy.Extensions.ServiceExtensions;

namespace VerEasy.Extensions.ServiceMiddlewares
{
    /// <summary>
    /// swagger中间件管道
    /// </summary>
    public static class SwaggerMiddleware
    {
        public static void UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(v =>
                {
                    x.SwaggerEndpoint($"swagger/{v}/swagger.json", $"{v}版本");
                });
                x.RoutePrefix = string.Empty;
            });
        }
    }
}