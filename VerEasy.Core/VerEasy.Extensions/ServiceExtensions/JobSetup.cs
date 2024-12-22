using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System.Reflection;
using VerEasy.Core.Tasks.Quartz.Net;

namespace VerEasy.Extensions.ServiceExtensions
{
    /// <summary>
    /// Task任务注入相关
    /// </summary>
    public static class JobSetup
    {
        public static void AddJobSetup(this IServiceCollection service)
        {
            service.AddSingleton<IJobFactory, JobFactory>();
            service.AddSingleton<IScheduleCenter, ScheduleCenter>();
            service.AddSingleton<IJobListener, JobListener>();

            //Job任务都继承于IJob接口
            var baseType = typeof(IJob);
            //获取根目录,用于查找Task.dll下的Job任务
            var basePath = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            //通过路径和名称找到指定的程序集
            var assembly = Directory.GetFiles(basePath, "VerEasy.Core.Tasks.dll")
                .Select(Assembly.LoadFrom);

            var types = assembly
                .SelectMany(x => x.DefinedTypes)//返回程序集里的类型集合[TypeInfo类型]
                .Select(x => x.AsType())//将TypeInfo类型转换为Type类型
                .Where(x => x != baseType && baseType.IsAssignableFrom(x));//过滤掉IJob类,只保留实现了IJob类的类型

            //真正要注入的Job类是Class类
            var injectTypes = types.Where(x => x.IsClass);

            foreach (var injectType in injectTypes)
            {
                service.AddTransient(injectType);
            }
        }
    }
}