using VerEasy.Core.IService.Base;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.IService.IService
{
    public interface IUserService : IBaseService<User>
    {
        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        Task<List<UserResult>> QueryUsersAsync();

        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<long> AddUserAsync(UserParam user);

        /// <summary>
        /// 根据ID删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUserByIdAsync(long id);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<MessageModel<bool>> UpdateUserAsync(UserParam user);

        /// <summary>
        /// 获取个人中心信息
        /// </summary>
        /// <returns></returns>
        Task<UserCenterResult> GetUserCenterInfo(long id);

        /// <summary>
        /// 个人中心编辑
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<bool>> EditUserCenterAsync(EditUserCenterParam param);

        /// <summary>
        /// 登录获取用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MessageModel<User>> LoginUserAsync(LoginParam param);
    }
}