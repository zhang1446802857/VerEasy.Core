using Autofac;
using Autofac.Extras.DynamicProxy;
using System.Reflection;
using VerEasy.Common.Helper;
using VerEasy.Common.Utils;
using VerEasy.Extensions.Aop;

namespace VerEasy.Extensions.ServiceExtensions.Module
{
    /// <summary>
    /// autofac注入相关,注入程序集和AOP
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            //程序集路径
            var repositoryPath = Path.Combine(basePath, "VerEasy.Core.Repository.dll");
            var servicePath = Path.Combine(basePath, "VerEasy.Core.Service.dll");

            var serviceAssembly = Assembly.LoadFrom(servicePath);
            var repositoryAssembly = Assembly.LoadFrom(repositoryPath);

            var types = new List<Type>();

            //LogAop启用
            if (Appsettings.App("ServiceSettings", "EnableAop", "LogAop").ObjToBool())
            {
                builder.RegisterType<LogAop>();
                types.Add(typeof(LogAop));
            }

            builder.RegisterAssemblyTypes(serviceAssembly)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(types.ToArray());

            builder.RegisterAssemblyTypes(repositoryAssembly)
               .AsImplementedInterfaces()
               .InstancePerDependency()
               .EnableInterfaceInterceptors();
        }
    }
}