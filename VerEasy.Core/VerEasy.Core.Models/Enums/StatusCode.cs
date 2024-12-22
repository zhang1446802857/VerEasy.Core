namespace VerEasy.Core.Models.Enums
{
    /// <summary>
    /// HTTP 状态码枚举，表示不同类型的响应状态
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// 请求成功,服务器成功处理了请求
        /// </summary>
        OK = 200,

        /// <summary>
        /// 请求失败，服务器无法理解
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// 客户端未通过身份验证，需认证
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 请求的资源未找到
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// 服务器遇到错误，无法完成请求
        /// </summary>
        InternalServerError = 500,
    }
}