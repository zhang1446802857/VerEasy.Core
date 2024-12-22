using SqlSugar;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Repository.Base;

namespace VerEasy.Core.IRepository.IRepository
{
    public class UserRepository(ISqlSugarClient db) : BaseRepository<User>(db), IUserRepository
    {
    }
}