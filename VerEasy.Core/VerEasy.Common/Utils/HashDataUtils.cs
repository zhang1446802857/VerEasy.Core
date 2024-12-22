namespace VerEasy.Common.Utils
{
    public class HashDataUtils
    {
        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string HashPwd(string pwd) => BCrypt.Net.BCrypt.HashPassword(pwd);

        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hashPwd"></param>
        /// <returns></returns>
        public static bool VerifyPwd(string input, string hashPwd) => BCrypt.Net.BCrypt.Verify(input, hashPwd);
    }
}