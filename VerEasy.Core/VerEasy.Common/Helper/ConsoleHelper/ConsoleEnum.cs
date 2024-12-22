namespace VerEasy.Common.Helper.ConsoleHelper
{
    public class ConsoleEnum
    {
        public enum Alignment
        {
            Left,
            Center,
            Right
        }

        public enum TableStyle
        {
            /// <summary>
            /// 默认格式的表格
            /// </summary>
            Default = 0,

            /// <summary>
            /// Markdwon格式的表格
            /// </summary>
            MarkDown = 1,

            /// <summary>
            /// 交替格式的表格
            /// </summary>
            Alternative = 2,

            /// <summary>
            /// 最简格式的表格
            /// </summary>
            Minimal = 3
        }
    }
}