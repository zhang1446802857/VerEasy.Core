using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Reflection;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.Tasks.Quartz.Net
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class ScheduleCenter : IScheduleCenter
    {
        /// <summary>
        /// 核心调度器
        /// </summary>
        private Task<IScheduler> scheduler;

        /// <summary>
        /// 任务工厂
        /// </summary>
        private readonly IJobFactory jobFactory;

        /// <summary>
        /// 监听器
        /// </summary>
        private readonly IJobListener jobListener;

        public ScheduleCenter(IJobFactory jobFactory, IJobListener jobListener)
        {
            this.jobFactory = jobFactory;
            this.jobListener = jobListener;
            scheduler = GetScheduleAsync();
        }

        /// <summary>
        /// 获取调度器
        /// </summary>
        /// <returns></returns>
        private Task<IScheduler> GetScheduleAsync()
        {
            if (this.scheduler != null)
            {
                return this.scheduler;
            }

            var scheduler = new StdSchedulerFactory(new System.Collections.Specialized.NameValueCollection
            {
                //{"quartz.jobStore.type","binary" }
            });
            var a = scheduler.GetScheduler();
            return scheduler.GetScheduler();
        }

        /// <summary>
        /// 添加一个Job任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MessageModel<string>> AddJobAsync(QzJobPlan job)
        {
            var result = new MessageModel<string>();
            if (job != null)
            {
                //通过数据库主键ID设置jobKey
                JobKey jobKey = new(job.Id.ToString(), job.JobGroup);
                //检验jobKey是否已经存在
                if (await scheduler.Result.CheckExists(jobKey))
                {
                    result.Success = false;
                    result.Message = $"该任务计划已经在执行:【{job.JobName}】,请勿重复启动！";
                }
                else
                {
                    job.JobBeginTime = job.JobBeginTime ?? DateTime.Now;
                    job.JobEndTime = job.JobEndTime ?? DateTime.MaxValue;

                    //通过程序集类名加载job的类型
                    var assembly = Assembly.Load(job.JobAssemblyName);
                    var jobType = assembly.GetType(job.JobAssemblyName + ".Quartz.Net.Jobs." + job.JobClassName);

                    //定义TaskJob
                    var taskJob = JobBuilder.Create(jobType)
                        .WithIdentity(job.Id.ToString(), job.JobGroup)
                        .Build();

                    //添加参数
                    taskJob.JobDataMap.Add("JobParams", job.JobParams);

                    //定时触发器
                    var trigger = CreateCronTrigger(job);

                    if (!scheduler.Result.IsStarted)
                    {
                        await StartScheduleAsync();
                    }

                    await scheduler.Result.ScheduleJob(taskJob, trigger);
                    result.Success = true;
                    result.Message = $"【{job.JobName}】成功!";
                }
            }
            else
            {
                result.Success = false;
                result.Message = "任务不存在,无法添加";
            }
            return result;
        }

        /// <summary>
        /// 开启任务调度器
        /// </summary>
        /// <returns></returns>
        public async Task<MessageModel<string>> StartScheduleAsync()
        {
            var result = new MessageModel<string>();
            try
            {
                scheduler.Result.JobFactory = jobFactory;
                if (!scheduler.Result.IsStarted)
                {
                    scheduler.Result.ListenerManager.AddJobListener(jobListener);
                    //开启调度器
                    await scheduler.Result.Start();
                }
                result.Success = true;
                result.Message = "任务调度器开启成功!";
                result.StatusCode = Models.Enums.StatusCode.OK;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 停止一个Job任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MessageModel<string>> StopJobAsync(QzJobPlan job)
        {
            var result = new MessageModel<string>();
            JobKey jobKey = new JobKey(job.Id.ToString(), job.JobGroup);

            try
            {
                if (await scheduler.Result.CheckExists(jobKey))
                {
                    await scheduler.Result.DeleteJob(jobKey);
                    result.Success = true;
                    result.Message = $"【{job.JobName}】已停止!";
                }
                else
                {
                    result.Success = false;
                    result.Message = $"【{job.JobName}】未找到!";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 关闭任务调度器
        /// </summary>
        /// <returns></returns>
        public async Task<MessageModel<string>> StopScheduleAsync()
        {
            var result = new MessageModel<string>();
            try
            {
                //true表示等待正在执行的任务结束后关闭
                await scheduler.Result.Shutdown(true);
                result.Success = true;
                result.Message = "任务调度器关闭成功!";
                result.StatusCode = Models.Enums.StatusCode.OK;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 暂停一个Job任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> PauseJobAsync(QzJobPlan job)
        {
            var result = new MessageModel<string>();
            JobKey jobKey = new(job.Id.ToString(), job.JobGroup);

            try
            {
                if (await scheduler.Result.CheckExists(jobKey))
                {
                    await scheduler.Result.PauseJob(jobKey);
                    result.Success = true;
                    result.Message = $"【{job.JobName}】已暂停!";
                }
                else
                {
                    result.Success = false;
                    result.Message = $"【{job.JobName}】未找到!";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 重启一个Job任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> RestartJobAsync(QzJobPlan job)
        {
            var result = new MessageModel<string>();
            JobKey jobKey = new JobKey(job.Id.ToString(), job.JobGroup);

            try
            {
                if (await scheduler.Result.CheckExists(jobKey))
                {
                    await scheduler.Result.ResumeJob(jobKey);
                    result.Success = true;
                    result.Message = $"【{job.JobName}】已重启!";
                }
                else
                {
                    result.Success = false;
                    result.Message = $"【{job.JobName}】未找到!";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 立即执行一个Job任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public async Task<MessageModel<string>> ExecuteJobAsync(QzJobPlan job)
        {
            var result = new MessageModel<string>();
            JobKey jobKey = new JobKey(job.Id.ToString(), job.JobGroup);

            try
            {
                //已有Job计划,立即执行一次
                if (await scheduler.Result.CheckExists(jobKey))
                {
                    await scheduler.Result.TriggerJob(jobKey);
                }
                else
                {
                    //无Job计划,新增一个然后执行,执行结束删除
                    await AddJobAsync(job);
                    await scheduler.Result.TriggerJob(jobKey);
                    await scheduler.Result.DeleteJob(jobKey);
                }
                result.Success = true;
                result.Message = $"【{job.JobName}】立即执行!";
            }
            catch (Exception)
            {
                result.Message = "立即执行失败!";
            }

            return result;
        }

        /// <summary>
        /// 创建Cron触发器
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        private static ITrigger CreateCronTrigger(QzJobPlan job)
        {
            var trigger = TriggerBuilder.Create()
                            .WithIdentity(job.Id.ToString(), job.JobGroup)
                            .WithCronSchedule(job.JobCron)
                            .ForJob(job.Id.ToString(), job.JobGroup);

            if (job.JobEndTime != null)
            {
                trigger.EndAt(job.JobEndTime.Value);
            }
            if (job.JobBeginTime != null)
            {
                trigger.StartAt(job.JobBeginTime.Value);
            }

            return trigger.Build();
        }
    }
}