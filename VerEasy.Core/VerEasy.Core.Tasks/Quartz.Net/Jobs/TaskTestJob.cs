using Quartz;
using VerEasy.Core.IService.IService;

namespace VerEasy.Core.Tasks.Quartz.Net.Jobs
{
    public class TaskTestJob(ITaskLogsService taskLogsService) : IJob
    {
        private readonly ITaskLogsService _taskLogsService = taskLogsService;

        public async Task Execute(IJobExecutionContext context)
        {
            await _taskLogsService.AddTestLogAsync();
        }
    }
}