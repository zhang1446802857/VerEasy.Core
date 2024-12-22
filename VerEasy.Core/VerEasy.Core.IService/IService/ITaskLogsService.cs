using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.IService.IService
{
    public interface ITaskLogsService : IBaseService<TaskLogs>
    {
        /// <summary>
        /// ���һ��������־
        /// </summary>
        /// <returns></returns>
        Task AddTestLogAsync();
    }
}