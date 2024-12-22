using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using VerEasy.Common.Helper;
using VerEasy.Common.Utils;
using VerEasy.Core.Models.Dtos;

namespace VerEasy.Core.Api.Filter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    /// <param name="logger"></param>
    public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger = logger;

        public object Model { get; private set; }

        public void OnException(ExceptionContext context)
        {
            var json = new MessageModel<string>
            {
                Message = context.Exception.Message,
                StatusCode = Models.Enums.StatusCode.InternalServerError
            };

            var res = new ContentResult()
            {
                Content = JsonSerializer.Serialize(json),
                ContentType = "application/json"
            };

            context.Result = res;

            #region 输出日志

            //不启用AOP时 使用过滤器记录
            if (!Appsettings.App("ServiceSettings", "EnableAop", "LogAop").ObjToBool())
            {
                var message = json.Message;
                var level = context.Exception.GetType().Name;
                _logger.LogError(context.Exception, "\r\n【异常类型】：{Level} \r\n【异常信息】：{LogMessage} \r\n", level, message);
            }

            #endregion 输出日志

            context.ExceptionHandled = true;
        }
    }
}