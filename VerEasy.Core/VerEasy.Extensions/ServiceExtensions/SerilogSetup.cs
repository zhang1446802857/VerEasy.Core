using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using VerEasy.Common.Helper;
using VerEasy.Common.LogHelper;
using VerEasy.Common.Options;
using VerEasy.Serilog;

namespace VerEasy.Extensions.ServiceExtensions
{
    /// <summary>
    /// serilog注入相关
    /// </summary>
    public static class SerilogSetup
    {
        public static IHostBuilder AddSerilogSetup(this IHostBuilder host)
        {
            ArgumentNullException.ThrowIfNull(host, nameof(host));
            var logConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(Appsettings.Configuration)
                .Enrich.FromLogContext()
                .WriteToConsole()//写出到控制台
                .WriteToFile()//写出到文件
                .WriteToDataBase();//写出到数据库(项目采用雪花ID,serilog自带的生成日志表是自增int不太符合项目,可根据需求选择)

            //var option = App.GetOptions<SeqOptions>();
            var option = Appsettings.App<SerilogOptions>("Seq");

            //配置Seq日志中心
            if (option.Enable)
            {
                var address = option.Address;
                var apiKey = option.Key;

                //logConfiguration =
                //    logConfiguration.WriteTo.Seq(address, restrictedToMinimumLevel: LogEventLevel.Verbose,
                //        apiKey: apiKey, eventBodyLimitBytes: 10485760);
            }

            Log.Logger = logConfiguration.CreateLogger();

            //Serilog 内部日志
            var file = File.CreateText(LogContextWrite.Combine($"SerilogDebug-{DateTime.Now:yyyyMMdd}.txt"));
            SelfLog.Enable(TextWriter.Synchronized(file));

            host.UseSerilog();
            return host;
        }
    }
}