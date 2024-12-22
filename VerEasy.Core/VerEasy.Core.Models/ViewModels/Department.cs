using SqlSugar;
using VerEasy.Core.Models.Base;

namespace VerEasy.Core.Models.ViewModels
{
    /// <summary>
    /// 部门表
    /// </summary>
    [SugarTable("T_Department")]
    public class Department : BaseModel
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上下级关系
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string SuperiorRelation { get; set; }

        /// <summary>
        /// 管理人员ID
        /// </summary>
        public long ManagerId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
}