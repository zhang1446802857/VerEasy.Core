using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Tasks.Quartz.Net;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(IScheduleCenter schedule, IQzJobPlanService qzJobPlanService) : ControllerBase
    {
        private readonly IScheduleCenter schedule = schedule;
        private IQzJobPlanService _qzJobPlanService = qzJobPlanService;

        [HttpGet("AddJob")]
        public async Task<MessageModel<string>> AddJob()
        {
            var result = new MessageModel<string>();
            try
            {
                QzJobPlan jobPlan = new QzJobPlan
                {
                    JobName = "加法计算",                         // 任务名称
                    JobGroup = "测试",                           // 任务分组
                    JobCron = "* * * * * ?",                   // Cron 表达式
                    JobClassName = "SumJob",                // 任务类名
                    JobAssemblyName = "VerEasy.Core.Tasks",     // 程序集名称
                    JobDescription = "加法计算任务",              // 任务描述
                    JobParams = "{\"param1\":\"value1\"}",       // 任务参数
                    Enable = true,                               // 启用状态
                    JobBeginTime = DateTime.Parse("2024-11-17 00:00:00"),  // 任务开始时间
                    JobEndTime = null,                           // 任务结束时间为 NULL
                    CreateTime = DateTime.Parse("2024-11-17 00:55:15.540"), // 创建时间
                    UpdateTime = DateTime.Parse("2024-11-17 00:55:15.540"), // 更新时间
                    CreateBy = 1,                                // 创建者 ID
                    UpdateBy = 1,                                // 更新者 ID
                    IsDeleted = false                            // 是否删除
                };
                await _qzJobPlanService.Add(jobPlan);
                await schedule.AddJobAsync(jobPlan);
                result.Message = $"【{jobPlan.JobName}】任务添加成功";
            }
            catch (Exception)
            {
                result.Message = $"任务添加失败";

                throw;
            }
            return result;
        }

        [HttpGet("StartJob")]
        public async Task<MessageModel<string>> StartJob(long jobId)
        {
            var result = new MessageModel<string>();
            try
            {
                QzJobPlan jobPlan = new QzJobPlan
                {
                    Id = 1234567890,                             // ID
                    JobName = "加法计算",                         // 任务名称
                    JobGroup = "测试",                           // 任务分组
                    JobCron = "0/1 * * * * ?",                   // Cron 表达式
                    JobClassName = "SumJob",                // 任务类名
                    JobAssemblyName = "VerEasy.Core.Tasks",     // 程序集名称
                    JobDescription = "加法计算任务",              // 任务描述
                    JobParams = "{\"param1\":\"value1\"}",       // 任务参数
                    Enable = true,                               // 启用状态
                    JobBeginTime = DateTime.Parse("2024-12-10 00:00:00"),  // 任务开始时间
                    JobEndTime = null,                           // 任务结束时间为 NULL
                    CreateTime = DateTime.Parse("2024-11-17 00:55:15.540"), // 创建时间
                    UpdateTime = DateTime.Parse("2024-11-17 00:55:15.540"), // 更新时间
                    CreateBy = 1,                                // 创建者 ID
                    UpdateBy = 1,                                // 更新者 ID
                    IsDeleted = false                            // 是否删除
                };
                await schedule.ExecuteJobAsync(jobPlan);
                result.Message = $"【{jobPlan.JobName}】任务执行成功";
            }
            catch (Exception)
            {
                result.Message = $"任务执行失败";

                throw;
            }
            return result;
        }

        [HttpGet("s")]
        public async Task S()
        {
            await schedule.StartScheduleAsync();
        }
    }
}