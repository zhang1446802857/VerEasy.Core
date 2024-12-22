using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.IService.IService
{
    public interface IDepartmentService : IBaseService<Department>
    {
        /// <summary>
        /// 新增部门信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<bool> AddDepartmentAsync(DepartmentParam param);

        /// <summary>
        /// 查询部门-嵌套格式
        /// </summary>
        /// <returns></returns>
        Task<List<CascaderResult>> QueryDepartmentsByCascaderAsync();

        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <returns></returns>
        Task<List<DepartmentResult>> QueryDepartmentResultsAsync();

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <returns></returns>
        Task<bool> EditDepartmentAsync(DepartmentParam param);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MessageModel<bool>> DeletedDepartmentByIdAsync(string id);
    }
}