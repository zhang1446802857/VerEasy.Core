using SqlSugar;
using VerEasy.Core.Models.Base;

namespace VerEasy.Core.Models.ViewModels
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("T_User")]
    public class User : BaseModel
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; } = "https://gitee.com/zh1446802857/library-storage/raw/master/848ab78f-eea0-4f88-a675-0b829e1f6b62.png";

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; } = "";

        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPwd { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; } = "";

        /// <summary>
        /// QQ号
        /// </summary>
        public string QQNumber { get; set; } = "";

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 个签-备注
        /// </summary>
        public string Remark { get; set; } = "";
    }
}