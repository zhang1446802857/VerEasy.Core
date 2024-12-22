using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IRoleService service) : ControllerBase
    {
        public IRoleService _service = service;

        [HttpPost("AddRoleAsync")]
        public async Task<MessageModel<bool>> AddRoleAsync(RoleParam role)
        {
            return MessageModel<bool>.Ok(await _service.AddRoleAsync(role));
        }

        [HttpPost("EditRoleAsync")]
        public async Task<MessageModel<bool>> EditRoleAsync(RoleParam role)
        {
            return await _service.EditRoleAsync(role);
        }

        [HttpGet("QueryRoleAsync")]
        public async Task<MessageModel<List<RoleResult>>> QueryRoleAsync()
        {
            return MessageModel<List<RoleResult>>.Ok(await _service.QueryRoleAsync());
        }

        [HttpGet("DeleteRoleAsync")]
        public async Task<MessageModel<bool>> DeleteRoleAsync(long id)
        {
            return MessageModel<bool>.Ok(await _service.DeleteRoleAsync(id));
        }
    }
}