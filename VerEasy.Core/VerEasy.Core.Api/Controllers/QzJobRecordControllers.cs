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
    public class QzJobRecordController(IQzJobRecordService service) : ControllerBase
    {
        public IQzJobRecordService _service = service;

        [HttpGet("QueryAll")]
        public async Task<MessageModel<List<QzJobRecord>>> QueryAll()
        {
            return MessageModel<List<QzJobRecord>>.Ok(await _service.Query());
        }
    }
}