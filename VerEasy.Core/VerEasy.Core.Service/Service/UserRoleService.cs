using VerEasy.Core.IRepository.Base;
using VerEasy.Core.IService.IService;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Service.Base;

namespace VerEasy.Core.Service.Service
{
    public class UserRoleService(IBaseRepository<UserRole> baseRepo) : BaseService<UserRole>(baseRepo), IUserRoleService
    {
    }
}