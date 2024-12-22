using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.IService.IService
{
    public interface IQzJobPlanService : IBaseService<QzJobPlan>
    {
        /// <summary>
        /// ��ѯ�����б�
        /// </summary>
        /// <returns></returns>
        Task<List<QzJobPlanResult>> QueryJobPlansAsync();

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        Task<bool> AddJobAsync(QzJobPlan job);

        /// <summary>
        /// ����ID����ִ��job����
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QzJobPlan> ExecuteJobAsync(long id);
    }
}