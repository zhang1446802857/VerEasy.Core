using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VerEasy.Serilog;

namespace VerEasy.Extensions.Aop
{
    public static class BaseLogAop
    {
        /// <summary>
        /// 等待无泛型返回值的异步方法完成并写入日志
        /// </summary>
        /// <param name="toWait">需要等待执行的异步方法</param>
        /// <param name="onSuceess">方法执行成功时执行的</param>
        /// <param name="onFail">方法执行失败时执行的</param>
        /// <returns></returns>
        public static async Task WaitTaskIsNoGenericity(Task toWait, Func<Task> onSuceess, Action<Exception> onFail)
        {
            Exception exception = null;
            try
            {
                await toWait;
                await onSuceess();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                onFail(exception);
            }
        }

        /// <summary>
        /// 等待有泛型返回值的异步方法完成并写入日志
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="toWait">需要等待执行的异步方法</param>
        /// <param name="onSuceess">方法执行成功时执行的</param>
        /// <param name="onFail">方法执行失败时执行的</param>
        /// <returns></returns>
        public static async Task<T> WaitTaskIsGenericity<T>(Task<T> toWait, Func<object, Task> onSuceess, Action<Exception> onFail)
        {
            Exception exception = null;
            try
            {
                var result = await toWait;
                await onSuceess(result);
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                onFail(exception);
            }
        }

        /// <summary>
        /// 调用等待泛型返回值的异步方法且写入日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="toWait"></param>
        /// <param name="onSuceess"></param>
        /// <param name="onFail"></param>
        /// <returns></returns>
        public static object CallWaitIsGenericity(Type type, object toWait, Func<object, Task> onSuceess, Action<Exception> onFail)
        {
            return typeof(BaseLogAop)
                .GetMethod("WaitTaskIsGenericity", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .MakeGenericMethod(type)
                .Invoke(null, [toWait, onSuceess, onFail]);
        }

        /// <summary>
        /// 成功时写入Info日志
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public static async Task LogInfoOnSuccess(IInvocation invocation, string userName, ILogger<LogAop> logger)
        {
            var infos = new BaseLogAopModel
            {
                ClassName = invocation.TargetType.Name,
                LogMessage = JsonConvert.SerializeObject(invocation.Method.Name + "执行成功"),
                MethodName = invocation.Method.Name,
                SourceContext = invocation.TargetType.UnderlyingSystemType.FullName,
                Operator = userName
            };
            var template = infos.BuildTemplate();

            await Task.Run(() => { logger.LogInformation(template.Item1, template.Item2); });
        }

        /// <summary>
        /// 失败时写入Info日志
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public static void LogErrorOnFail(Exception exception, IInvocation invocation, string userName, ILogger<LogAop> logger)
        {
            if (exception != null)
            {
                var infos = new BaseLogAopModel
                {
                    ClassName = invocation.TargetType.Name,
                    LogMessage = exception.Message,
                    MethodName = invocation.Method.Name,
                    SourceContext = invocation.TargetType.UnderlyingSystemType.FullName,
                    Operator = userName
                };
                var template = infos.BuildTemplate();
                logger.LogError(exception, template.Item1, template.Item2);
            }
        }
    }
}