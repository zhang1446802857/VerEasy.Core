using VerEasy.Common.Helper;
using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Service.Base;

namespace VerEasy.Core.Service.Service
{
    public class TaskLogsService(IBaseRepository<TaskLogs> baseRepo) : BaseService<TaskLogs>(baseRepo), ITaskLogsService
    {
        public async Task AddTestLogAsync()
        {
            var log = new TaskLogs()
            {
                LogMessage = poetryHelper.Hstring(),
                TaskName = "TASK"
            };
            await Add(log);
        }
    }
}