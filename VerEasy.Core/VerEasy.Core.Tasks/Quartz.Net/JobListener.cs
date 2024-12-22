using Quartz;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.Tasks.Quartz.Net
{
    public class JobListener(IQzJobRecordService qzJobRecordService) : IJobListener
    {
        private readonly IQzJobRecordService _qzJobRecordService = qzJobRecordService;

        /// <summary>
        /// 监听器名称
        /// </summary>
        public string Name => "JobListener";

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            var jobId = long.Parse(context.JobDetail.Key.Name);
            var log = new QzJobRecord
            {
                JobExcuteTime = DateTime.Now,
                JobId = jobId,
                JobExcuteMsg = $"Task执行方法:{context.JobDetail.JobType.Name}"
            };

            await _qzJobRecordService.Add(log);
        }
    }
}