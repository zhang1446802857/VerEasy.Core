using System.Text;
using static VerEasy.Common.Helper.ConsoleHelper.ConsoleEnum;

namespace VerEasy.Common.Helper.ConsoleHelper
{
    /// <summary>
    /// 控制台输出table类
    /// </summary>
    public class ConsoleTable
    {
        #region 属性

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public IList<string> Columns
        {
            get
            {
                if (_columns == null)
                {
                    return [];
                }
                return _columns;
            }
            set
            {
                _columns = value;
                _columnsWidth = [];
            }
        }

        /// <summary>
        /// 行
        /// </summary>
        public List<string[]> Rows { get; set; } = [];

        /// <summary>
        /// 输入列宽
        /// </summary>
        public List<int> ColumsWidthInput { get; set; } = [];

        /// <summary>
        /// 空白符数量
        /// </summary>
        public int ColumsBlankNumber { get; set; } = 5;

        /// <summary>
        /// 对齐方式
        /// </summary>
        public Alignment Alignment { get; set; } = Alignment.Left;

        /// <summary>
        /// 显示行数
        /// </summary>
        public bool EnableCount { get; set; } = false;

        /// <summary>
        /// 表格样式
        /// </summary>
        public TableStyle TableStyle
        {
            get
            {
                return _tableStyle;
            }
            set
            {
                if (_tableStyle == value) return;
                _tableStyle = value;
                _formatInfo = null;
            }
        }

        private IList<string> _columns;//列
        private List<int> _columnsWidth = [];//列宽
        private List<int> _finalColumnWidths = [];//最终列宽
        private TableStyle _tableStyle;//表格样式
        private StyleInfo _formatInfo;//显式输出样式
        private readonly List<ColumnShowFormat> _columnShowFormats = [];//显式输出样式

        /// <summary>
        /// 显式输出样式
        /// </summary>
        private StyleInfo FormatInfo
        {
            get
            {
                _formatInfo ??= _tableStyle.GetFormatInfo();
                return _formatInfo;
            }
            set
            {
                _formatInfo = value;
            }
        }

        /// <summary>
        /// 最终列宽
        /// </summary>
        private List<int> FinalColumsWidths
        {
            get
            {
                if (_columnsWidth is null || _columnsWidth.Count < 1)
                {
                    // 得到每一列最大的宽度
                    List<int> _columnWidthMax = Columns.GetColumnWidth(Rows);
                    // 替换用户输入长度
                    ColumsWidthInput ??= [];
                    // 用户输入列宽覆盖自动计算宽度
                    for (int i = 0; i < ColumsWidthInput.Count; i++)
                    {
                        _columnWidthMax[i] = ColumsWidthInput[i];
                    }
                    _finalColumnWidths = _columnWidthMax;
                }
                return _finalColumnWidths;
            }
        }

        /// <summary>
        /// 显式输出样式
        /// </summary>
        private List<ColumnShowFormat> ColumnShowFormats
        {
            get
            {
                if (_columnShowFormats.Count == 0)
                {
                    for (int i = 0; i < Columns.Count; i++)
                    {
                        _columnShowFormats.Add(new ColumnShowFormat(i, FinalColumsWidths[i], Alignment));
                    }
                }
                return _columnShowFormats;
            }
        }

        #endregion 属性

        public void Write(ConsoleColor color = ConsoleColor.White)
        {
            ConsoleExtension.WriteColorLine(GetHeader(), color);
            ConsoleExtension.WriteInfoLine(GetExistData());
            ConsoleExtension.WriteColorLine(GetEnd(), color);
        }

        #region 内容获取

        /// <summary>
        /// 获取完成头
        /// </summary>
        /// <returns></returns>
        public string GetHeader()
        {
            // 创建顶部和底部分隔线
            string top_DownDividerdivider = FinalColumsWidths.GetTopAndDwon(FormatInfo.AngleStr, ColumsBlankNumber);
            // 创建分隔线
            string divider = FinalColumsWidths.GetDivider(FormatInfo.AngleStr, ColumsBlankNumber);
            // 获取标题字符串
            string tilte = FinalColumsWidths.GetTitleStr(Title, ColumsBlankNumber, FormatInfo.DelimiterStr);
            // 得到头部字符串
            string headers = ColumnShowFormats.FillFormatTostring(Columns.ToArray(), FormatInfo.DelimiterStr, ColumsBlankNumber);

            //绘制表格头
            StringBuilder top = new();
            if (FormatInfo.IsShowTop_Down_DataBorder) top.AppendLine(top_DownDividerdivider);
            if (!string.IsNullOrWhiteSpace(tilte))
            {
                top.AppendLine(tilte);
                top.AppendLine(divider);
            }
            top.AppendLine(headers);
            top.AppendLine(divider);
            return top.ToString().Trim();
        }

        /// <summary>
        /// 获取现有数据
        /// </summary>
        /// <returns></returns>
        public string GetExistData()
        {
            // 创建分隔线
            string divider = FinalColumsWidths.GetDivider(FormatInfo.AngleStr, ColumsBlankNumber);
            // 得到每行数据的字符串
            List<string> rowStrs = Rows.Select(row => ColumnShowFormats.FillFormatTostring(row, FormatInfo.DelimiterStr, ColumsBlankNumber)).ToList();
            StringBuilder data = new();
            for (int i = 0; i < rowStrs.Count; i++)
            {
                if (FormatInfo.IsShowTop_Down_DataBorder && i != 0) data.AppendLine(divider);
                data.AppendLine(rowStrs[i]);
            }
            return data.ToString().Trim();
        }

        /// <summary>
        /// 获取底
        /// </summary>
        /// <returns></returns>
        public string GetEnd()
        {
            StringBuilder down = new();
            if (FormatInfo.IsShowTop_Down_DataBorder) down.AppendLine(FinalColumsWidths.GetTopAndDwon(FormatInfo.AngleStr, ColumsBlankNumber));
            if (EnableCount) down.AppendLine($" Count: {Rows.Count}");
            return down.ToString().Trim();
        }

        #endregion 内容获取
    }

    public class StyleInfo(string delimiterStr = "|", bool isShowTopDownDataBorder = true, string angleStr = "-")
    {
        /// <summary>
        /// 每一列数据之间的间隔字符串
        /// </summary>
        public string DelimiterStr { get; set; } = delimiterStr;

        /// <summary>
        /// 是否显示顶部，底部，和每一行数据之间的横向边框
        /// </summary>
        public bool IsShowTop_Down_DataBorder { get; set; } = isShowTopDownDataBorder;

        /// <summary>
        /// 边角字符串
        /// </summary>
        public string AngleStr { get; set; } = angleStr;
    }
}