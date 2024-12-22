using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VerEasy.Common.Helper;
using VerEasy.Common.Helper.ConsoleHelper;
using VerEasy.Common.Utils;
using static VerEasy.Common.Helper.ConsoleHelper.ConsoleEnum;

namespace VerEasy.Extensions.ServiceExtensions
{
    /// <summary>
    /// 用于控制台输出信息,主要是输出该项目的各种配置信息等
    /// </summary>
    public static class AppConfigConsoleSetup
    {
        public static void AddAppConfigConsoleSetup(this IServiceCollection services, IHostEnvironment env)
        {
            ArgumentNullException.ThrowIfNull(services);
            if (Appsettings.App("ServiceSettings", "EnableAppInfoConsole").ObjToBool())
            {
                var infosService = Appsettings.Apps("AppConfig:Features");
                List<string[]> servicesInfos =
                [
                    ["当前环境",Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")],
                ];
                foreach (var info in infosService)
                {
                    servicesInfos.Add([info.Key, info.Value]);
                }
                new ConsoleTable()
                {
                    Title = $"{Appsettings.App("AppConfig", "ApiName")} 配置列表",
                    Columns = ["配置名称", "配置信息"],
                    Rows = servicesInfos,
                    TableStyle = TableStyle.Alternative
                }.Write(ConsoleColor.Cyan);
            }
        }
    }
}