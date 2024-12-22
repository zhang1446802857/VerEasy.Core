using SqlSugar;
using VerEasy.Core.Models.Base;

namespace VerEasy.Core.Models.ViewModels
{
    [SugarTable("T_UserRole")]
    public class UserRole : BaseModel
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}