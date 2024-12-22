using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.Tasks.Quartz.Net
{
    /// <summary>
    /// 调度中心接口:用于操作JOB的启动,添加…
    /// </summary>
    public interface IScheduleCenter
    {
        /// <summary>
        /// 开启任务调度,开启Quartz内部的定时任务
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<string>> StartScheduleAsync();

        /// <summary>
        /// 停止任务调度:停止Quartz内部的定时任务
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<string>> StopScheduleAsync();

        /// <summary>
        /// 添加一个Job任务
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<string>> AddJobAsync(QzJobPlan job);

        /// <summary>
        /// 停止一个Job任务
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<string>> StopJobAsync(QzJobPlan job);

        /// <summary>
        /// 暂停一个Job任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        Task<MessageModel<string>> PauseJobAsync(QzJobPlan job);

        /// <summary>
        /// 重启一个Job任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        Task<MessageModel<string>> RestartJobAsync(QzJobPlan job);

        /// <summary>
        /// 立即执行一个Job任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        Task<MessageModel<string>> ExecuteJobAsync(QzJobPlan job);
    }
}