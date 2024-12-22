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
    public class UserDepartmentController(IUserDepartmentService service) : ControllerBase
    {
        public IUserDepartmentService _service = service;

        [HttpGet("QueryAll")]
        public async Task<MessageModel<List<UserDepartment>>> QueryAll()
        {
            return MessageModel<List<UserDepartment>>.Ok(await _service.Query());
        }
    }
}