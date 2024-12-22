using AutoMapper;
using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.Dtos;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Service.Base;
using static VerEasy.Core.Models.Dtos.ParamDto;
using static VerEasy.Core.Models.Dtos.ResultDto;

namespace VerEasy.Core.Service.Service
{
    public class RoleService(IBaseRepository<Role> baseRepo, IMapper mapper) : BaseService<Role>(baseRepo), IRoleService
    {
        public async Task<bool> AddRoleAsync(RoleParam role)
        {
            var newRole = mapper.Map<Role>(role);
            if (await Add(newRole) > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteRoleAsync(long id)
        {
            return await DeleteById(id);
        }

        public async Task<MessageModel<bool>> EditRoleAsync(RoleParam role)
        {
            var result = await QueryById(role.Id);
            if (result != null)
            {
                if (!string.IsNullOrEmpty(role.Name))
                {
                    result.Name = role.Name;
                }
                if (!string.IsNullOrEmpty(role.Description))
                {
                    result.Description = role.Description;
                }
                return MessageModel<bool>.Ok(await Update(result));
            }
            return MessageModel<bool>.Fail("查询不到该角色,请刷新尝试");
        }

        public async Task<List<RoleResult>> QueryRoleAsync()
        {
            var result = await Query(x => !x.IsDeleted);
            return mapper.Map<List<RoleResult>>(result);
        }
    }
}