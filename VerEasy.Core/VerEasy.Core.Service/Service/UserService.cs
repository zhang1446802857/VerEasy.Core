using AutoMapper;
using VerEasy.Common.Utils;
using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Service.Base;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Service.Service
{
    public class UserService(IBaseRepository<User> baseRepo, IMapper mapper) : BaseService<User>(baseRepo), IUserService
    {
        public async Task<long> AddUserAsync(UserParam user)
        {
            user.Pwd = HashDataUtils.HashPwd(user.Pwd);
            var userInfo = mapper.Map<User>(user);
            return await Add(userInfo);
        }

        public async Task<bool> DeleteUserByIdAsync(long id)
        {
            return await DeleteById(id);
        }

        public async Task<MessageModel<bool>> EditUserCenterAsync(EditUserCenterParam param)
        {
            var result = await QueryById(param.Id);
            if (result != null)
            {
                try
                {
                    mapper.Map(param, result);
                    await Update(result);
                    return MessageModel<bool>.Ok("�޸ĳɹ�");
                }
                catch (Exception)
                {
                    return MessageModel<bool>.Fail("�޸�ʧ��");
                }
            }
            return MessageModel<bool>.Fail("δ�ҵ��û�");
        }

        public async Task<UserCenterResult> GetUserCenterInfo(long id)
        {
            var user = await QueryById(id);
            var result = mapper.Map<UserCenterResult>(user);
            return result;
        }

        public async Task<MessageModel<User>> LoginUserAsync(LoginParam param)
        {
            //���ݵ�¼����ȡ�û�
            var result = (await Query(x => x.LoginName == param.LoginName && !x.IsDeleted)).FirstOrDefault();
            if (result == null)
            {
                return MessageModel<User>.Fail("δ�ҵ���¼��Ϊ" + param.LoginName + "���û�");
            }
            //У������
            if (!HashDataUtils.VerifyPwd(param.LoginPwd, result.LoginPwd))
            {
                return MessageModel<User>.Fail("�������,����������");
            }

            return MessageModel<User>.Ok(result);
        }

        public async Task<List<UserResult>> QueryUsersAsync()
        {
            var result = await Query(x => !x.IsDeleted);
            var resultDto = mapper.Map<List<UserResult>>(result);
            return resultDto;
        }

        public async Task<MessageModel<bool>> UpdateUserAsync(UserParam user)
        {
            var info = await QueryById(user.Id);
            if (info != null)
            {
                if (!string.IsNullOrEmpty(user.Pwd))
                {
                    info.LoginPwd = user.Pwd;
                }
                if (!string.IsNullOrEmpty(user.UserName))
                {
                    info.UserName = user.UserName;
                }
                info.Enable = user.Enable;
                return MessageModel<bool>.Ok(await Update(info));
            }
            return MessageModel<bool>.Fail("���޴���");
        }
    }
}