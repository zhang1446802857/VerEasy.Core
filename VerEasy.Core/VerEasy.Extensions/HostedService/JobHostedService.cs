using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VerEasy.Common.Helper;
using VerEasy.Common.Utils;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Tasks.Quartz.Net;

namespace VerEasy.Extensions.HostedService
{
    public class JobHostedService(IQzJobPlanService qzJobPlanService, IScheduleCenter schedule, ILogger<JobHostedService> logger) : IHostedService
    {
        private readonly IQzJobPlanService _qzJobPlanService = qzJobPlanService;
        private readonly IScheduleCenter _schedule = schedule;
        private readonly ILogger<JobHostedService> _logger = logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Task任务调度启动!");
            await DoWork();
        }

        private async Task DoWork()
        {
            try
            {
                if (Appsettings.App("ServiceSettings", "EnableTaskJob").ObjToBool())
                {
                    var jobs = await _qzJobPlanService.Query(x => !x.IsDeleted);
                    foreach (var item in jobs)
                    {
                        if (item.Enable)
                        {
                            var result = await _schedule.AddJobAsync(item);
                            if (result.Success)
                            {
                                Console.WriteLine($"JOB{item.JobName}启动成功!");
                            }
                            else
                            {
                                Console.WriteLine($"JOB{item.JobName}启动失败!错误原因:{result.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Task任务调度失败!");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Task任务调度结束!");
            return Task.CompletedTask;
        }
    }
}