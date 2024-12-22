using SqlSugar;
using VerEasy.Core.Models.Base;
using VerEasy.Core.Models.Enums;

namespace VerEasy.Core.Models.ViewModels
{
    /// <summary>
    /// 菜单管理页面
    /// </summary>
    [SugarTable("T_Permission")]
    public class Permission : BaseModel
    {
        /// <summary>
        /// 菜单/按钮/页面名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组件路径
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 类型[0按钮,1页面,2菜单]
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 上下级关系
        /// </summary>
        [SugarColumn(Length = 2000)]
        public string SuperiorRelation { get; set; }

        /// <summary>
        /// 是否有加载框
        /// </summary>
        public bool Loading { get; set; }

        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; } = "Menu";
    }
}