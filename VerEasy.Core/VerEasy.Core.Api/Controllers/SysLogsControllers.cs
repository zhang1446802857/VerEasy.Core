using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysLogsController(ISysLogsService service) : ControllerBase
    {
        public ISysLogsService _service = service;

        [HttpGet("QueryTaskTestLogsAsync")]
        public async Task<MessageModel<List<SysLogs>>> QueryTaskTestLogsAsync()
        {
            return MessageModel<List<SysLogs>>.Ok(await _service.QueryTaskTestLogsAsync());
        }
    }
}