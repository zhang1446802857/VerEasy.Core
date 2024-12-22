using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController(IPermissionService service) : ControllerBase
    {
        public IPermissionService _service = service;

        [HttpGet("QueryMenusAsync")]
        public async Task<MessageModel<List<PermissionResult>>> QueryMenusAsync()
        {
            return MessageModel<List<PermissionResult>>.Ok(await _service.QueryMenusAsync());
        }

        [HttpPost("AddMenuAsync")]
        public async Task<MessageModel<bool>> AddMenu(PermissionParam menu)
        {
            return MessageModel<bool>.Ok(await _service.AddMenuAsync(menu));
        }

        [HttpPost("EditMenuAsync")]
        public async Task<MessageModel<bool>> EditMenuAsync(PermissionParam menu)
        {
            return await _service.EditMenuAsync(menu);
        }

        [HttpGet("DeleteMenusAsync")]
        public async Task<MessageModel<bool>> DeleteMenusAsync(long id)
        {
            return MessageModel<bool>.Ok(await _service.DeleteMenusAsync(id));
        }

        [HttpGet("QueryMenusByCascaderAsync")]
        public async Task<MessageModel<List<CascaderResult>>> QueryMenusByCascaderAsync()
        {
            return MessageModel<List<CascaderResult>>.Ok(await _service.QueryMenusByCascaderAsync());
        }
    }
}