namespace VerEasy.Core.Models.Dtos
{
    public class PageModel<T>
    {
        public PageModel()
        { }

        public PageModel(int pageIndex, int pageSize, int totalCount, List<T> datas)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Datas = datas;
        }

        /// <summary>
        /// 页标
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> Datas { get; set; }
    }
}