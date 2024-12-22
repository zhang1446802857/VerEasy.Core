using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskLogsController(ITaskLogsService service) : ControllerBase
    {
        public ITaskLogsService _service = service;

        [HttpGet("QueryAll")]
        public async Task<MessageModel<List<TaskLogs>>> QueryAll()
        {
            return MessageModel<List<TaskLogs>>.Ok(await _service.Query());
        }
    }
}