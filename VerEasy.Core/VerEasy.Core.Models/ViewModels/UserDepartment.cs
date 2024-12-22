using SqlSugar;
using VerEasy.Core.Models.Base;

namespace VerEasy.Core.Models.ViewModels
{
    [SugarTable("T_UserDepartment")]
    public class UserDepartment : BaseModel
    {
        public long UserId { get; set; }
        public long DepartmentId { get; set; }
    }
}