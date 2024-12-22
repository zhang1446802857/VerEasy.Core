using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.IService.IService
{
    public interface IQzJobPlanService : IBaseService<QzJobPlan>
    {
        /// <summary>
        /// 查询任务列表
        /// </summary>
        /// <returns></returns>
        Task<List<QzJobPlanResult>> QueryJobPlansAsync();

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        Task<bool> AddJobAsync(QzJobPlan job);

        /// <summary>
        /// 根据ID立刻执行job任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QzJobPlan> ExecuteJobAsync(long id);
    }
}