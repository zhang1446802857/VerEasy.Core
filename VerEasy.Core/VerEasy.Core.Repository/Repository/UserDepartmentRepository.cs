using SqlSugar;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Repository.Base;

namespace VerEasy.Core.IRepository.IRepository
{
    public class UserDepartmentRepository(ISqlSugarClient db) : BaseRepository<UserDepartment>(db), IUserDepartmentRepository
    {
    }
}