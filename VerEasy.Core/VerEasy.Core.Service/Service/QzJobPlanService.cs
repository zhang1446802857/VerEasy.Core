using AutoMapper;
using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Service.Base;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Service.Service
{
    public class QzJobPlanService(IBaseRepository<QzJobPlan> baseRepo, IMapper mapper) : BaseService<QzJobPlan>(baseRepo), IQzJobPlanService
    {
        public async Task<bool> AddJobAsync(QzJobPlan job)
        {
            return await Add(job) > 0;
        }

        public async Task<QzJobPlan> ExecuteJobAsync(long id)
        {
            return await QueryById(id);
        }

        public async Task<List<QzJobPlanResult>> QueryJobPlansAsync()
        {
            return mapper.Map<List<QzJobPlanResult>>(await Query(x => !x.IsDeleted));
        }
    }
}