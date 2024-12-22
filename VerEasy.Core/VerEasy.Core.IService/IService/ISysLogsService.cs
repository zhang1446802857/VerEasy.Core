using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.IService.IService
{
    public interface ISysLogsService : IBaseService<SysLogs>
    {
        /// <summary>
        /// ≤È—ØTask≤‚ ‘»’÷æ
        /// </summary>
        /// <returns></returns>
        Task<List<SysLogs>> QueryTaskTestLogsAsync();

        Task<int> Sun(int a, int b);
    }
}