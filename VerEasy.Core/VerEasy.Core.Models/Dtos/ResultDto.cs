using VerEasy.Core.Models.Enums;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.Models.Dtos
{
    public class ResultDto
    {
        public class QzJobPlanResult : QzJobPlan
        {
            public new string Id { get; set; }
            public int ExecuteNums { get; set; }

            public string JobRecord { get; set; }
        }

        public class DepartmentResult
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string CreateTime { get; set; }
            public string UpdateTime { get; set; }
            public bool Enable { get; set; }
            public string ParentName { get; set; }
            public string SuperiorRelation { get; set; }
            public List<DepartmentResult> Children { get; set; }
        }

        public class CascaderResult
        {
            /// <summary>
            /// 部门名称
            /// </summary>
            public string Label { get; set; }

            /// <summary>
            /// ID
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// 子级部门
            /// </summary>
            public List<CascaderResult> Children { get; set; }
        }

        public class UserResult
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string CreateTime { get; set; }
            public string UpdateTime { get; set; }
            public bool Enable { get; set; }
            public string Email { get; set; }
            public string Remark { get; set; }
            public string[] RoleNames { get; set; }
            public string[] DepartmentNames { get; set; }
            public string[] RoleIds { get; set; }
        }

        public class RoleResult
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool Enable { get; set; }
            public string Description { get; set; }
            public string CreateTime { get; set; }
            public string UpdateTime { get; set; }
        }

        public class PermissionResult
        {
            /// <summary>
            /// 菜单/按钮名称
            /// </summary>
            public string Title { get; set; }

            public string Name { get; set; }

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
            /// 类型[0按钮,1菜单]
            /// </summary>
            public PermissionType Type { get; set; }

            /// <summary>
            /// 是否启用
            /// </summary>
            public bool Enable { get; set; }

            public string Id { get; set; }
            public string CreateTime { get; set; }
            public string UpdateTime { get; set; }
            public string SuperiorRelation { get; set; }
            public List<PermissionResult> Children { get; set; }
        }

        public class VueRouterResult
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public string Component { get; set; }
            public string Redirect { get; set; }
            public List<VueRouterResult> Children { get; set; }
            public Meta Meta { get; set; }
        }

        public class Meta
        {
            public string Title { get; set; }
            public string Icon { get; set; }
            public bool Loading { get; set; }
        }

        /// <summary>
        /// 个人中心
        /// </summary>
        public class UserCenterResult
        {
            /// <summary>
            /// 头像
            /// </summary>
            public string Avatar { get; set; }

            /// <summary>
            /// 登录名
            /// </summary>
            public string LoginName { get; set; }

            /// <summary>
            /// 用户昵称
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// 真实姓名
            /// </summary>
            public string RealName { get; set; }

            /// <summary>
            /// 邮箱
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// 手机号
            /// </summary>
            public string PhoneNumber { get; set; }

            /// <summary>
            /// QQ号
            /// </summary>
            public string QQNumber { get; set; }

            /// <summary>
            /// 个签-备注
            /// </summary>
            public string Remark { get; set; }

            public string Id { get; set; }
        }
    }
}