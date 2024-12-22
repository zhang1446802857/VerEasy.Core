using Microsoft.Extensions.Logging;
using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Service.Base;

namespace VerEasy.Core.Service.Service
{
    public class SysLogsService(IBaseRepository<SysLogs> baseRepo, ILogger<SysLogsService> logger) : BaseService<SysLogs>(baseRepo), ISysLogsService
    {
        private readonly ILogger<SysLogsService> _logger = logger;

        public async Task<List<SysLogs>> QueryTaskTestLogsAsync()
        {
            return await Query();
        }

        public async Task<int> Sun(int a, int b)
        {
            return await Task.Run(() =>
            {
                return (int)a + (int)b;
            });
        }
    }
}