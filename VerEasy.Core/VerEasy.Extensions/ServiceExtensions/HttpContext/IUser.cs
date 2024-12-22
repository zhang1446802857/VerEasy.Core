namespace VerEasy.Extensions.ServiceExtensions.HttpContext
{
    /// <summary>
    /// 关联HTTPCONTEX的接口,用于注入程序
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// ID
        /// </summary>
        long Id { get; }

        /// <summary>
        /// 是否已验证
        /// </summary>
        /// <returns></returns>
        bool IsAuthenticated();

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        string GetToken();

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <returns></returns>
        long GetUid();
    }
}