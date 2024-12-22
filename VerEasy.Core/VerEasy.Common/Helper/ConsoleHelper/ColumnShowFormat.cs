using static VerEasy.Common.Helper.ConsoleHelper.ConsoleEnum;

namespace VerEasy.Common.Helper.ConsoleHelper
{
    /// <summary>
    /// 列渲染格式
    /// </summary>
    public class ColumnShowFormat(int index, int strLength, ConsoleEnum.Alignment alignment)
    {
        /// <summary>
        /// 索引，第几列数据
        /// </summary>
        public int Index { get; set; } = index;

        /// <summary>
        /// 对其方式
        /// </summary>
        public Alignment Alignment { get; set; } = alignment;

        /// <summary>
        /// 一列字符串长度
        /// </summary>
        public int StrLength { get; set; } = strLength;
    }
}