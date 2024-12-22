using VerEasy.Core.Models.Enums;

namespace VerEasy.Core.Models.Dtos
{
    public class MessageModel
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public StatusCode Status { get; set; } = StatusCode.OK;

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 返回数据集合
        /// </summary>
        public object Response { get; set; }
    }

    /// <summary>
    /// 通用返回信息类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageModel<T>
    {
        /// <summary>
        /// 状态码:[http请求层次]
        /// </summary>
        public StatusCode StatusCode { get; set; } = StatusCode.OK;

        /// <summary>
        /// 操作是否成功[业务层次]
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 返回数据内容
        /// </summary>
        public T Response { get; set; }

        /// <summary>
        /// 返回成功[只返回信息]
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static MessageModel<T> Ok(string msg)
        {
            return Messages(true, msg, default);
        }

        /// <summary>
        /// 返回成功[默认OK,返回数据]
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static MessageModel<T> Ok(T response)
        {
            return Messages(true, "操作成功!", response);
        }

        /// <summary>
        /// 返回成功[带数据返回]
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static MessageModel<T> Ok(string msg, T response)
        {
            return Messages(true, msg, response);
        }

        /// <summary>
        /// 返回失败[只返回信息]
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static MessageModel<T> Fail(string msg)
        {
            return Messages(false, msg, default);
        }

        /// <summary>
        /// 返回失败[带数据返回]
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static MessageModel<T> Fail(string msg, T response)
        {
            return Messages(false, msg, response);
        }

        public static MessageModel<T> Messages(bool success, string msg, T? response)
        {
            return new MessageModel<T>()
            {
                Success = success,
                Message = msg,
                Response = response
            };
        }
    }

    public static class MessageModelConvert
    {
        public static MessageModel<TNew> ConvertTo<T, TNew>(this MessageModel<T> messageModel)
        {
            if (messageModel == null)
            {
                return MessageModel<TNew>.Fail("转换失败，原MessageModel为null");
            }

            return new MessageModel<TNew>
            {
                StatusCode = messageModel.StatusCode,
                Success = messageModel.Success,
                Message = messageModel.Message,
            };
        }
    }
}