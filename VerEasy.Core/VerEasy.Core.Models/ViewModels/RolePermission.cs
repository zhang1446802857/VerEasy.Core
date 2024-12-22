using SqlSugar;
using VerEasy.Core.Models.Base;

namespace VerEasy.Core.Models.ViewModels
{
    /// <summary>
    /// 角色权限关系表
    /// </summary>
    [SugarTable("T_RolePermission")]
    public class RolePermission : BaseModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public long PermissionId { get; set; }
    }
}