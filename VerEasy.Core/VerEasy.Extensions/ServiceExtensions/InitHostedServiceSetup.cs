using Microsoft.Extensions.DependencyInjection;

namespace VerEasy.Extensions.ServiceExtensions
{
    /// <summary>
    /// 初始化hosted
    /// </summary>
    public static class InitHostedServiceSetup
    {
        public static void AddInitHostedServiceSetup(this IServiceCollection services)
        {
            //services.AddHostedService<JobHostedService>();
        }
    }
}