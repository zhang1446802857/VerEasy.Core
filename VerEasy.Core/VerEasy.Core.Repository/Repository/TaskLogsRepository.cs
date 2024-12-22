using SqlSugar;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Repository.Base;

namespace VerEasy.Core.IRepository.IRepository
{
    public class TaskLogsRepository(ISqlSugarClient db) : BaseRepository<TaskLogs>(db), ITaskLogsRepository
    {
    }
}