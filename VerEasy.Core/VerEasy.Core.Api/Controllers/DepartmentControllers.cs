using Microsoft.AspNetCore.Mvc;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(IDepartmentService service) : ControllerBase
    {
        public IDepartmentService _service = service;

        [HttpGet("QueryAll")]
        public async Task<MessageModel<List<Department>>> QueryAll()
        {
            return MessageModel<List<Department>>.Ok(await _service.Query());
        }

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost("AddDepartmentAsync")]
        public async Task<MessageModel<bool>> AddDepartmentAsync(DepartmentParam department)
        {
            if (await _service.AddDepartmentAsync(department))
            {
                return MessageModel<bool>.Ok("添加成功");
            }
            return MessageModel<bool>.Fail("添加失败");
        }

        /// <summary>
        /// 查询部门-嵌套格式
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryDepartmentsByCascaderAsync")]
        public async Task<MessageModel<List<CascaderResult>>> QueryDepartmentsByCascaderAsync()
        {
            var result = await _service.QueryDepartmentsByCascaderAsync();
            return MessageModel<List<CascaderResult>>.Ok(result);
        }

        /// <summary>
        /// 查询部门
        /// </summary>
        /// <returns></returns>
        [HttpGet("QueryDepartmentResultsAsync")]
        public async Task<MessageModel<List<DepartmentResult>>> QueryDepartmentResultsAsync()
        {
            var result = await _service.QueryDepartmentResultsAsync();
            return MessageModel<List<DepartmentResult>>.Ok(result);
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost("EditDepartmentAsync")]
        public async Task<MessageModel<bool>> EditDepartmentAsync(DepartmentParam department)
        {
            var result = await _service.EditDepartmentAsync(department);
            return MessageModel<bool>.Ok(result);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("DeletedDepartmentByIdAsync")]
        public async Task<MessageModel<bool>> DeletedDepartmentByIdAsync(string id)
        {
            return await _service.DeletedDepartmentByIdAsync(id);
        }
    }
}