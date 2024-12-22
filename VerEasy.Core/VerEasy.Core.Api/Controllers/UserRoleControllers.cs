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
    public class UserRoleController(IUserRoleService service) : ControllerBase
    {
        public IUserRoleService _service = service;

        [HttpGet("QueryAll")]
        public async Task<MessageModel<List<UserRole>>> QueryAll()
        {
            return MessageModel<List<UserRole>>.Ok(await _service.Query());
        }
    }
}