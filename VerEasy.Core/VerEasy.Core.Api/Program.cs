using Autofac;
using Autofac.Extensions.DependencyInjection;
using VerEasy.Common.Helper;
using VerEasy.Core.Api.Filter;
using VerEasy.Extensions.ServiceExtensions;
using VerEasy.Extensions.ServiceExtensions.Module;
using VerEasy.Extensions.ServiceMiddlewares;

namespace VerEasy.Core.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            //将配置文件注入Appsettings,用于全局获取配置信息
            builder.Services.AddSingleton(new Appsettings(builder.Configuration));
            builder.Services.AddSwaggerSetup();

            //服务注入
            builder.Host.AddSerilogSetup();
            builder.Services.AddAppConfigConsoleSetup(builder.Environment);
            builder.Services.AddSqlsugarSetup();
            builder.Services.AddJobSetup();
            builder.Services.AddAuthorizationByJwtSetup();
            builder.Services.AddCors(x =>
            {
                x.AddPolicy("default", a =>
                {
                    a.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            builder.Services.AddAutoMapper(typeof(MappingProfileModule));
            builder.Services.AddHttpContextUserSetup();
            builder.Services.AddInitHostedServiceSetup();

            //Aotofac扩展注入
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(x =>
                {
                    x.RegisterModule<AutofacModuleRegister>();
                });

            //过滤器注入
            builder.Services.AddControllers(x =>
            {
                x.Filters.Add<GlobalExceptionFilter>();
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddleware();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("default");

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}