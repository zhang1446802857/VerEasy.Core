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
        /// ��ѯ�û�
        /// </summary>
        /// <returns></returns>
        Task<List<UserResult>> QueryUsersAsync();

        /// <summary>
        /// ������û�
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<long> AddUserAsync(UserParam user);

        /// <summary>
        /// ����IDɾ���û�
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUserByIdAsync(long id);

        /// <summary>
        /// �޸��û���Ϣ
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<MessageModel<bool>> UpdateUserAsync(UserParam user);

        /// <summary>
        /// ��ȡ����������Ϣ
        /// </summary>
        /// <returns></returns>
        Task<UserCenterResult> GetUserCenterInfo(long id);

        /// <summary>
        /// �������ı༭
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<bool>> EditUserCenterAsync(EditUserCenterParam param);

        /// <summary>
        /// ��¼��ȡ�û���Ϣ
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<MessageModel<User>> LoginUserAsync(LoginParam param);
    }
}