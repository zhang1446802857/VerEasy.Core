using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.IService.IService
{
    public interface ITaskLogsService : IBaseService<TaskLogs>
    {
        /// <summary>
        /// 添加一条测试日志
        /// </summary>
        /// <returns></returns>
        Task AddTestLogAsync();
    }
}