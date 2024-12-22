using System.Text.RegularExpressions;
using VerEasy.Common.Utils;
using static VerEasy.Common.Helper.ConsoleHelper.ConsoleEnum;

namespace VerEasy.Common.Helper.ConsoleHelper
{
    /// <summary>
    /// 控制台输出扩展类
    /// </summary>
    public static class ConsoleExtension
    {
        #region 控制台输出

        private static readonly object _objLock = new();

        /// <summary>
        /// 输出有颜色的文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        public static void WriteColorLine(string str, ConsoleColor color)
        {
            lock (_objLock)
            {
                ConsoleColor currentForeColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(str);
                Console.ForegroundColor = currentForeColor;
            }
        }

        /// <summary>
        /// 打印错误信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteErrorLine(this string str, ConsoleColor color = ConsoleColor.Red) => WriteColorLine(str, color);

        /// <summary>
        /// 打印警告信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteWarningLine(this string str, ConsoleColor color = ConsoleColor.Yellow) => WriteColorLine(str, color);

        /// <summary>
        /// 打印正常信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteInfoLine(this string str, ConsoleColor color = ConsoleColor.White) => WriteColorLine(str, color);

        /// <summary>
        /// 打印成功的信息
        /// </summary>
        /// <param name="str">待打印的字符串</param>
        /// <param name="color">想要打印的颜色</param>
        public static void WriteSuccessLine(this string str, ConsoleColor color = ConsoleColor.Green) => WriteColorLine(str, color);

        #endregion 控制台输出

        /// <summary>
        /// 获取显式样式
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static StyleInfo GetFormatInfo(this TableStyle format)
        {
            return format switch
            {
                TableStyle.Default => new StyleInfo("|", true, "-"),
                TableStyle.MarkDown => new StyleInfo("|", false, "|"),
                TableStyle.Alternative => new StyleInfo("|", true, "+"),
                TableStyle.Minimal => new StyleInfo("", false, "-"),
                _ => new StyleInfo(),
            };
        }

        /// <summary>
        /// 按照现有数据计算每列最大宽度
        /// </summary>
        /// <param name="columns">列信息</param>
        /// <param name="rows">现有行数据</param>
        /// <returns>每一列显示宽度</returns>
        public static List<int> GetColumnWidth(this IList<string> columns, IList<string[]> rows)
        {
            List<int> columnLengths = columns.Select((t, i) =>
            rows.Select(x => x[i])//得到所有行当前列的数据
            .Union(new[] { columns[i] })//连接当前列标题
            .Where(x => x != null)
            .Select(x => x.ObjToString().FullHalfLength())//得到该列每一行的字符串长度(计算中文占用两格)
            .Max())//到该列中长度最大的以列
            .ToList();
            return columnLengths;
        }

        /// <summary>
        /// 获取文本长度，区分全角半角 全角算两个字符
        /// </summary>
        /// <returns></returns>
        public static int FullHalfLength(this string text)
        {
            return Regex.Replace(text, "[^\x00-\xff]", "**").Length;
        }

        /// <summary>
        /// 获取头部和底部字符串
        /// </summary>
        /// <param name="columnWides">列宽信息</param>
        /// <param name="angleStr">每列之间分割字符串</param>
        /// <param name="columnBlankNum">列两端留白数</param>
        /// <returns></returns>
        public static string GetTopAndDwon(this List<int> columnWides, string angleStr, int columnBlankNum)
        {
            string top_DownDividerdivider = "";
            columnWides.ForEach(i =>
            {
                if (string.IsNullOrWhiteSpace(top_DownDividerdivider)) top_DownDividerdivider += $"{angleStr}{"".PadRight(i + columnBlankNum * 2, '-')}";
                else top_DownDividerdivider += $"{"".PadRight(i + columnBlankNum * 2 + 1, '-')}";
            });
            top_DownDividerdivider += angleStr;
            return top_DownDividerdivider;
        }

        /// <summary>
        /// 获取每行之间的分割行字符串
        /// </summary>
        /// <param name="columnWides">列宽信息</param>
        /// <param name="angleStr">每列之间分割字符串</param>
        /// <param name="columnBlankNum">列两端留白数</param>
        /// <returns></returns>
        public static string GetDivider(this List<int> columnWides, string angleStr, int columnBlankNum)
        {
            string divider = "";
            columnWides.ForEach(i =>
            {
                divider += $"{angleStr}{"".PadRight(i + columnBlankNum * 2, '-')}";
            });
            divider += angleStr;
            return divider;
        }

        /// <summary>
        /// 获取title 字符串
        /// </summary>
        /// <param name="columnWides">&gt;</param>
        /// <param name="titleStr">标题字符串信息</param>
        /// <param name="columnBlankNum">列两端留白数</param>
        /// <param name="delimiterStr">每列之间分割字符串</param>
        /// <returns></returns>
        public static string GetTitleStr(this List<int> columnWides, string titleStr, int columnBlankNum, string delimiterStr)
        {
            if (string.IsNullOrWhiteSpace(titleStr)) return "";
            //一行的宽度
            int rowWide = columnWides.Sum() + columnWides.Count * 2 * columnBlankNum + columnWides.Count + 1;
            int blankNum = (rowWide - titleStr.FullHalfLength()) / 2 - 1;
            string tilte = $"{delimiterStr}{"".PadLeft(blankNum, ' ')}{titleStr}{"".PadLeft(blankNum, ' ')}{delimiterStr}";
            if (tilte.FullHalfLength() != rowWide) tilte = tilte.Replace($" {delimiterStr}", $"  {delimiterStr}");
            return tilte;
        }

        /// <summary>
        /// 将填充格式转成字符串 表头和数据行会用到
        /// </summary>
        /// <param name="format">一行的显示格式信息</param>
        /// <param name="objs">一行要显示的数据</param>
        /// <param name="delimiterStr">间隔符</param>
        /// <param name="columnBlankNum">每列留白数</param>
        /// <returns></returns>
        public static string FillFormatTostring(this List<ColumnShowFormat> format, string[] objs, string delimiterStr, int columnBlankNum)
        {
            string formatStr = string.Empty;
            format.ForEach(f =>
            {
                string ali = f.Alignment == Alignment.Right ? "" : "-";
                string val = objs[f.Index].ObjToString();
                if (val.Length > f.StrLength)
                {
                    objs[f.Index] = "...";//标记超出长度
                }

                if (!string.IsNullOrWhiteSpace(formatStr)) formatStr += $"{"".PadLeft(columnBlankNum, ' ')}";
                int alignmentStrLength = Math.Max(f.StrLength - objs[f.Index].ObjToString().GetChineseText().Length, 0);//对其填充空格数量
                formatStr += $"{delimiterStr}{"".PadLeft(columnBlankNum, ' ')}{{{f.Index},{ali}{alignmentStrLength}}}";
            });
            formatStr += $"{"".PadLeft(columnBlankNum, ' ')}{delimiterStr}";
            return string.Format(formatStr, objs);
        }

        /// <summary>
        /// 获取中文文本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetChineseText(this string text) => Regex.Replace(text, "[\x00-\xff]", "");
    }
}