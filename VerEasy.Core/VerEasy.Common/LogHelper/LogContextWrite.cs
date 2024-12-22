namespace VerEasy.Common.LogHelper
{
    public class LogContextWrite
    {
        static LogContextWrite()
        {
            //创建本地日志默认目录
            if (!Directory.Exists(BaseLogs))
            {
                Directory.CreateDirectory(BaseLogs);
            }
        }

        public static readonly string BaseLogs = "Logs";

        public static string Combine(string path1)
        {
            return Path.Combine(BaseLogs, path1);
        }
    }
}