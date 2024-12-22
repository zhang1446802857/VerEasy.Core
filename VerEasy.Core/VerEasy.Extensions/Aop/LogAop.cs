using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System.Reflection;
using VerEasy.Extensions.ServiceExtensions.HttpContext;

namespace VerEasy.Extensions.Aop
{
    public class LogAop(ILogger<LogAop> logger, IUser user) : IInterceptor
    {
        private readonly ILogger<LogAop> logger = logger;
        private readonly string userName = user.UserName;

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
                //异步方法处理
                if (IsAsyncMethod(invocation.Method))
                {
                    if (invocation.Method.ReturnType == typeof(Task))
                    {
                        invocation.ReturnValue = BaseLogAop.WaitTaskIsNoGenericity(
                            (Task)invocation.ReturnValue,
                            async () => await BaseLogAop.LogInfoOnSuccess(invocation, userName, logger),
                            ex =>
                            {
                                BaseLogAop.LogErrorOnFail(ex, invocation, userName, logger);
                            });
                    }
                    else
                    {
                        invocation.ReturnValue = BaseLogAop.CallWaitIsGenericity(
                            invocation.Method.ReturnType.GenericTypeArguments[0],
                            invocation.ReturnValue,
                            async (o) =>
                            {
                                await BaseLogAop.LogInfoOnSuccess(invocation, userName, logger);
                            },
                            ex =>
                            {
                                BaseLogAop.LogErrorOnFail(ex, invocation, userName, logger);
                            });
                    }
                }
                else//同步方法
                {
                }
            }
            catch (Exception ex)
            {
                BaseLogAop.LogErrorOnFail(ex, invocation, userName, logger);
            }
        }

        /// <summary>
        /// 判断是否是异步的方法
        /// </summary>
        /// <returns></returns>
        private static bool IsAsyncMethod(MethodInfo method) => (typeof(Task) == method.ReturnType ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)));
    }
}