using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace VerEasy.Core.Tasks.Quartz.Net
{
    public class JobFactory(IServiceProvider serviceProvider) : IJobFactory
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var serviceScope = serviceProvider.CreateScope();
                var job = serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ReturnJob(IJob job)
        {
            if (job is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}