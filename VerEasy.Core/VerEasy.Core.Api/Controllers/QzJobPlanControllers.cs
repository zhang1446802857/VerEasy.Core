using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Tasks.Quartz.Net;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QzJobPlanController(ITaskLogsService taskLogsService, IQzJobRecordService qzJobRecordService, IMapper mapper, IQzJobPlanService service, IScheduleCenter schedule) : ControllerBase
    {
        private readonly ITaskLogsService _taskLogsService = taskLogsService;
        private readonly IQzJobRecordService _qzJobRecordService = qzJobRecordService;
        private readonly IMapper _mapper = mapper;
        public IQzJobPlanService _service = service;
        private readonly IScheduleCenter _schedule = schedule;

        /// <summary>
        /// 查询Job列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryJobPlansAsync")]
        public async Task<MessageModel<List<QzJobPlanResult>>> QueryJobPlansAsync()
        {
            var result = await _service.QueryJobPlansAsync();
            var records = await _qzJobRecordService.Query(x => result.Select(c => long.Parse(c.Id)).Contains(x.JobId));

            foreach (var item in result)
            {
                var record = records.Where(x => x.JobId.ToString() == item.Id).OrderByDescending(a => a.JobExcuteTime)
                    .Select(x => $"执行时间:{x.JobExcuteTime},{x.JobExcuteMsg}");
                item.ExecuteNums = record.Count();
                item.JobRecord = string.Join("\r\n", record);
            }

            return MessageModel<List<QzJobPlanResult>>.Ok(result);
        }

        /// <summary>
        /// 添加一个Job
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [HttpPost("AddJobAsync")]
        public async Task<MessageModel<bool>> AddJobAsync(QzJobParam job)
        {
            var result = await _service.AddJobAsync(job);
            if (result)
            {
                if (job.Enable)
                {
                    await _schedule.AddJobAsync(job);
                }
                return MessageModel<bool>.Ok(true);
            }
            else
            {
                return MessageModel<bool>.Fail("添加失败");
            }
        }

        /// <summary>
        /// 编辑一个Job
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [HttpPost("EditJobAsync")]
        public async Task<MessageModel<bool>> EditJobAsync(QzJobParam job)
        {
            var result = await _service.QueryById(job.Id);
            //停止原任务
            await _schedule.StopJobAsync(result);
            //修改原任务
            _mapper.Map(job, result);

            if (await _service.Update(result))
            {
                //如果是启动状态,直接添加到任务列表进行执行
                if (job.Enable)
                {
                    await _schedule.AddJobAsync(result);
                }
                return MessageModel<bool>.Ok(true);
            }

            return MessageModel<bool>.Fail("编辑失败");
        }

        /// <summary>
        /// 立刻执行一个任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ExecuteJobAsync")]
        public async Task<MessageModel<string>> ExecuteJobAsync(long id)
        {
            var result = await _service.QueryById(id);
            if (result != null)
            {
                return await _schedule.ExecuteJobAsync(result);
            }
            return MessageModel<string>.Fail("任务未找到!");
        }

        /// <summary>
        /// 启动一个定时任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("StartJobAsync")]
        public async Task<MessageModel<string>> StartJobAsync(long id)
        {
            var result = await _service.QueryById(id);
            if (result != null)
            {
                result.Enable = true;
                await _service.Update(result);
                return await _schedule.AddJobAsync(result);
            }
            return MessageModel<string>.Fail("任务未找到!");
        }

        /// <summary>
        /// 停止一个定时任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("StopJobAsync")]
        public async Task<MessageModel<string>> StopJobAsync(long id)
        {
            var result = await _service.QueryById(id);
            if (result != null)
            {
                result.Enable = false;
                await _service.Update(result);
                return await _schedule.StopJobAsync(result);
            }
            return MessageModel<string>.Fail("任务未找到!");
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("DeleteJobsAsync")]
        public async Task<MessageModel<bool>> DeleteJobsAsync([FromBody] long[] ids)
        {
            try
            {
                var lists = await _service.Query(x => ids.Contains(x.Id));
                //中止任务
                foreach (var list in lists)
                {
                    await _schedule.StopJobAsync(list);
                }
                await _service.Delete(lists);
            }
            catch (Exception ex)
            {
                return MessageModel<bool>.Fail("删除失败:" + ex.Message);
            }
            return MessageModel<bool>.Ok("删除成功");
        }
    }
}