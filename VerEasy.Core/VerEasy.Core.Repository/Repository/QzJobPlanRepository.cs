using SqlSugar;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Repository.Base;

namespace VerEasy.Core.IRepository.IRepository
{
    public class QzJobPlanRepository(ISqlSugarClient db) : BaseRepository<QzJobPlan>(db), IQzJobPlanRepository
    {
    }
}