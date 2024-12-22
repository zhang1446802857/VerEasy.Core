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

            //�������ļ�ע��Appsettings,����ȫ�ֻ�ȡ������Ϣ
            builder.Services.AddSingleton(new Appsettings(builder.Configuration));
            builder.Services.AddSwaggerSetup();

            //����ע��
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

            //Aotofac��չע��
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(x =>
                {
                    x.RegisterModule<AutofacModuleRegister>();
                });

            //������ע��
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