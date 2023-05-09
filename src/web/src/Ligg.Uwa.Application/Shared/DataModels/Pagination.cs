namespace Ligg.Uwa.Application.Shared
{
    public class Pagination
    {
        public Pagination()
        {
            SortName = "Id"; // 默认按Id排序
            SortType = "desc";
            PageIndex = 1;
            PageSize = 15;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortName { get; set; }
        public string SortType { get; set; }
        public int Total { get; set; }

        public int PageTotal
        {
            get
            {
                if (Total > 0)
                {
                    return Total % this.PageSize == 0 ? Total / this.PageSize : Total / this.PageSize + 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
