using SqlSugar;
using VerEasy.Core.Models.ViewModels;
using VerEasy.Core.Repository.Base;

namespace VerEasy.Core.IRepository.IRepository
{
    public class PermissionRepository(ISqlSugarClient db) : BaseRepository<Permission>(db), IPermissionRepository
    {
    }
}