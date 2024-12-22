namespace VerEasy.Serilog
{
    /// <summary>
    /// 基础的日志配置相关
    /// </summary>
    public class BaseLogAopModel
    {
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 日志信息
        /// </summary>
        public string LogMessage { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string SourceContext { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public string Duration { get; set; }

        public static readonly string LogTemplate = "\r\n【方法名】：{MethodName} \r\n【操作人】：{Operator} \r\n【日志信息】：{LogMessage} \r\n【来源】：{SourceContext}\r\n【类名】：{ClassName}\r\n【耗时】：{Duration}";

        public (string, object[]) BuildTemplate()
        {
            // 检查字段是否为 null 或空，若是则使用默认值
            var methodName = string.IsNullOrEmpty(MethodName) ? "未知方法" : MethodName;
            var operatorName = string.IsNullOrEmpty(Operator) ? "未知操作人" : Operator;
            var logMessage = string.IsNullOrEmpty(LogMessage) ? "无日志信息" : LogMessage;
            var sourceContext = string.IsNullOrEmpty(SourceContext) ? "无来源" : SourceContext;
            var className = string.IsNullOrEmpty(ClassName) ? "未知类" : ClassName;
            var duration = string.IsNullOrEmpty(Duration) ? "未知耗时" : Duration;

            return (LogTemplate, new object[] { methodName, operatorName, logMessage, sourceContext, className, duration });
        }
    }
}