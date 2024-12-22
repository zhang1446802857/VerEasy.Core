using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Extensions.ServiceExtensions.HttpContext;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service, IUser iuser, IUserRoleService userRoleService, IUserDepartmentService userDepartment, IDepartmentService departmentService, IRoleService roleService) : ControllerBase
    {
        public IUserService _service = service;
        private readonly IUser _iuser = iuser;
        private readonly IUserRoleService _userRoleService = userRoleService;
        private readonly IRoleService _roleService = roleService;
        private readonly IUserDepartmentService _userDepartmentService = userDepartment;
        private readonly IDepartmentService _departmentService = departmentService;

        [HttpGet("QueryUsersAsync")]
        public async Task<MessageModel<List<UserResult>>> QueryUsersAsync()
        {
            var users = await _service.QueryUsersAsync();
            var userids = users.Select(x => long.Parse(x.Id)).ToList();

            var roles = await _userRoleService.Query(x => !x.IsDeleted && userids.Contains(x.UserId));
            var roleids = roles.Select(x => x.RoleId).ToList();
            var rolenames = await _roleService.Query(x => roleids.Contains(x.Id) && !x.IsDeleted);

            var departments = await _userDepartmentService.Query(x => !x.IsDeleted && userids.Contains(x.UserId));
            var departmentIds = departments.Select(x => x.DepartmentId).ToList();
            var departmentNames = await _departmentService.Query(x => departmentIds.Contains(x.Id) && !x.IsDeleted);

            users.ForEach(x =>
            {
                // 获取当前用户的所有角色 ID
                var userRoleIds = roles.Where(r => r.UserId == long.Parse(x.Id)).Select(r => r.RoleId);
                // 获取与这些角色 ID 关联的角色名
                var userRoleNames = rolenames.Where(r => userRoleIds.Contains(r.Id)).Select(r => r.Name.Trim()).OrderBy(x => x.Length).ToArray();
                // 将角色名集合赋值给用户的 RoleNames 属性
                x.RoleNames = userRoleNames;

                var userDepartmentIds = departments.Where(r => r.UserId == long.Parse(x.Id)).Select(r => r.DepartmentId);
                var userDepartmentNames = rolenames.Where(r => userDepartmentIds.Contains(r.Id)).Select(r => r.Name.Trim()).OrderBy(x => x.Length).ToArray();
                x.DepartmentNames = userDepartmentNames;

                x.RoleIds = userRoleIds.Select(x => x.ToString()).ToArray();
            });

            return MessageModel<List<UserResult>>.Ok(users);
        }

        [HttpPost("AddUserAsync")]
        public async Task<MessageModel<bool>> AddUserAsync(UserParam user)
        {
            var userId = await _service.AddUserAsync(user);
            var userRole = user.Roles.Select(x => new UserRole { UserId = userId, RoleId = x }).ToList();
            await _userRoleService.Add(userRole);
            if (user.Departments != null)
            {
                var userDepartment = user.Departments.Select(x => new UserDepartment { UserId = userId, DepartmentId = x }).ToList();
                await _userDepartmentService.Add(userDepartment);
            }
            return MessageModel<bool>.Ok(true);
        }

        [HttpGet("DeleteUserByIdAsync")]
        public async Task<MessageModel<bool>> DeleteUserByIdAsync(long id)
        {
            if (await _service.DeleteUserByIdAsync(id))
            {
                return MessageModel<bool>.Ok("删除成功");
            }
            return MessageModel<bool>.Fail("删除失败");
        }

        [HttpPost("UpdataUserAsync")]
        public async Task<MessageModel<bool>> UpdataUserAsync(UserParam user)
        {
            //转换集合
            var userRole = user.Roles.Select(x => new UserRole { UserId = user.Id, RoleId = x }).ToList();
            var userDepartment = user.Departments.Select(x => new UserDepartment { UserId = user.Id, DepartmentId = x }).ToList();
            //当前用户的角色/部门
            var existingRoles = await _userRoleService.Query(x => x.UserId == user.Id && !x.IsDeleted);
            var existingDepartments = await _userDepartmentService.Query(x => x.UserId == user.Id && !x.IsDeleted);
            // 1. 获取需要新增的角色/部门
            var newRoles = userRole.Where(x => !existingRoles.Any(dbRole => dbRole.RoleId == x.RoleId)).ToList();
            var newDepartments = userDepartment.Where(x => !existingDepartments.Any(department => department.DepartmentId == x.DepartmentId)).ToList();
            // 2. 获取需要删除的角色/部门
            var rolesToRemove = existingRoles.Where(dbRole => !userRole.Any(x => x.RoleId == dbRole.RoleId)).ToList();
            var departmentToRemove = existingDepartments.Where(x => !userDepartment.Any(x => x.DepartmentId == x.DepartmentId)).ToList();
            // 新增数据到数据库
            if (newRoles.Count != 0)
            {
                await _userRoleService.Add(newRoles);
            }
            if (newDepartments.Count != 0)
            {
                await _userDepartmentService.Add(newDepartments);
            }

            // 删除数据库中多余的角色/部门
            if (rolesToRemove.Count != 0)
            {
                await _userRoleService.Delete(rolesToRemove);
            }
            if (departmentToRemove.Count != 0)
            {
                await _userDepartmentService.Delete(departmentToRemove);
            }

            return await _service.UpdateUserAsync(user);
        }

        [HttpGet("GetUserCenterInfo")]
        public async Task<MessageModel<UserCenterResult>> GetUserCenterInfo()
        {
            return MessageModel<UserCenterResult>.Ok(await _service.GetUserCenterInfo(_iuser.Id));
        }

        [HttpPost("EditUserCenterAsync")]
        public async Task<MessageModel<bool>> EditUserCenterAsync(EditUserCenterParam param)
        {
            return await _service.EditUserCenterAsync(param);
        }
    }
}