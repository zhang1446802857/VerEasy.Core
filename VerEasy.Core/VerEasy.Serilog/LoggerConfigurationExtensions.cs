using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using VerEasy.Common.Helper;
using VerEasy.Common.LogHelper;

namespace VerEasy.Serilog
{
    /// <summary>
    /// 扩展日志的输出方式配置
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        private static readonly string LogTemplate = "{NewLine}日期：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}日志等级：{Level}{NewLine}信息：{Message}{NewLine}【堆栈调用】：{Exception}" + new string('-', 100);

        private static readonly string[] DefaultExcludingConditions =
        [
            "Request finished", "Sending", "Request starting", "Executing", "Route matched with", "Executed "
        ];

        private static readonly string[] ExcludingConditions =
        [
            "Content", "Hosting", "Application", "listening"
        ];

        /// <summary>
        /// 输出日志到Console[Debug]
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static LoggerConfiguration WriteToConsole(this LoggerConfiguration configuration)
        {
            configuration = configuration.WriteTo.Logger(x =>
            {
                x.Filter.ByExcluding(ExcludingCondition())
                .Filter.ByExcluding(a => a.Level != LogEventLevel.Debug)
                .WriteTo.Console();
            });
            return configuration;
        }

        /// <summary>
        /// 输出日志到本地文件[Info/Error]
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <returns></returns>
        public static LoggerConfiguration WriteToFile(this LoggerConfiguration configuration)
        {
            //输出INFO日志
            configuration = configuration.WriteTo.Logger(x =>
            {
                x.Filter.ByIncludingOnly(x => x.Level == LogEventLevel.Information)
                .Filter.ByExcluding(ExcludingCondition())
                .WriteTo.Async(s => s.File(LogContextWrite.Combine(@"INFO/.txt"), rollingInterval: RollingInterval.Day, outputTemplate: LogTemplate));
            });
            //输出ERROR日志
            configuration = configuration.WriteTo.Logger(x =>
            {
                x.Filter.ByIncludingOnly(x => x.Level == LogEventLevel.Error)
                .Filter.ByExcluding(ExcludingCondition())
                .WriteTo.Async(s => s.File(LogContextWrite.Combine(@"ERROR/.txt"), rollingInterval: RollingInterval.Day, outputTemplate: LogTemplate));
            });
            //输出Debug日志
            configuration = configuration.WriteTo.Logger(x =>
            {
                x.Filter.ByIncludingOnly(x => x.Level == LogEventLevel.Debug)
                .Filter.ByExcluding(ExcludingCondition())
                .WriteTo.Async(s => s.File(LogContextWrite.Combine(@"DEBUG/.txt"), rollingInterval: RollingInterval.Day, outputTemplate: LogTemplate));
            });
            return configuration;
        }

        /// <summary>
        /// 输出日志到数据库[Info/Error]
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static LoggerConfiguration WriteToDataBase(this LoggerConfiguration configuration)
        {
            var databaseSettings = Appsettings.App<DatabaseSettings>("DatabaseSettings");
            var defaultDb = databaseSettings.Db.FirstOrDefault(x => x.Name == databaseSettings.DefaultDb);

            configuration = configuration.WriteTo.Logger(x =>
            {
                //.Filter.ByExcluding(l => l.Level != LogEventLevel.Error && l.Level != LogEventLevel.Information)
                x.Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .Filter.ByExcluding(ExcludingCondition())
                .Filter.ByExcluding(l => l.Properties.ContainsKey("SourceContext") && l.Properties["SourceContext"].ToString().Contains("Quartz.")) // 根据 SourceContext 过滤
                .WriteTo.MSSqlServer(defaultDb.ConnectionString, new MSSqlServerSinkOptions
                {
                    AutoCreateSqlTable = true,
                    TableName = Appsettings.App("Seq:WriteToDataBaseTable"),
                });
            });
            return configuration;
        }

        /// <summary>
        /// 日志过滤器:根据内容进行过滤
        /// </summary>
        /// <param name="values">过滤值</param>
        /// <returns></returns>
        public static Func<LogEvent, bool> ExcludingCondition(params string[] values)
        {
            values = [.. DefaultExcludingConditions, .. ExcludingConditions, .. values];
            return log =>
            {
                return values.Any(x => log.MessageTemplate.Text.Contains(x));
            };
        }

        #region 配置类

        public class DatabaseSettings
        {
            public bool EnableMultiple { get; set; } // 是否启用多库操作
            public string DefaultDb { get; set; } // 默认数据库名称
            public List<DbConfig> Db { get; set; } // 数据库列表
        }

        public class DbConfig
        {
            public string Name { get; set; } // 数据库名称
            public string DbTypeName { get; set; } // 数据库类型名称
            public string ConnectionString { get; set; } // 数据库连接字符串
            public bool Enable { get; set; }
        }

        #endregion 配置类
    }
}