using VerEasy.Core.Models.Enums;
using VerEasy.Core.Models.ViewModels;

namespace VerEasy.Core.Models.Dtos
{
    public class ParamDto
    {
        public class RouterParam
        {
            public long[] Ids { get; set; }
            public bool All { get; set; }
        }

        public class QzJobParam : QzJobPlan
        {
            public new string JobBeginTime { get; set; }
            public new string JobEndTime { get; set; }
        }

        public class DepartmentParam
        {
            /// <summary>
            /// 部门名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 上下级关系
            /// </summary>
            public string SuperiorRelation { get; set; }

            /// <summary>
            /// 管理人员ID
            /// </summary>
            public long ManagerId { get; set; }

            /// <summary>
            /// ID
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// 是否启用
            /// </summary>
            public bool Enable { get; set; }
        }

        public class UserParam
        {
            /// <summary>
            /// ID
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// 是否启用
            /// </summary>
            public bool Enable { get; set; }

            /// <summary>
            /// 密码
            /// </summary>
            public string Pwd { get; set; }

            /// <summary>
            /// 登录名
            /// </summary>
            public string LoginName { get; set; }

            /// <summary>
            /// 角色组
            /// </summary>
            public long[] Roles { get; set; }

            /// <summary>
            /// 部门组
            /// </summary>
            public long[] Departments { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }

            /// <summary>
            /// 用户昵称
            /// </summary>

            public string UserName { get; set; }
        }

        public class RoleParam
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public bool Enable { get; set; }
        }

        public class PermissionParam
        {
            public long Id { get; set; }

            /// <summary>
            /// 菜单/按钮标题名称
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 组件路径
            /// </summary>
            public string Component { get; set; }

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

            /// <summary>
            /// 子父级关系
            /// </summary>
            public string SuperiorRelation { get; set; }
        }

        public class RolePermissionParam
        {
            public long RoleId { get; set; }
            public long[] PermissionIds { get; set; }
        }

        public class EditUserCenterParam
        {
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

            public long Id { get; set; }
        }

        public class LoginParam
        {
            public string LoginName { get; set; }

            public string LoginPwd { get; set; }
        }
    }
}