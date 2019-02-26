namespace CompareHare.Domain.ql
{
    public class QueryPage
    {
        private QueryPage(int rowOffset, int pageSize)
        {
            RowOffset = rowOffset;
            PageSize = pageSize;
        }

        public int PageSize { get; }
        public int RowOffset { get; }

        public static QueryPage FromRowOffsetAndPageSize(int rowOffset, int pageSize)
        {
            return new QueryPage(rowOffset, pageSize);
        }

        public static QueryPage FromPageNumberAndSize(int pageNumber, int pageSize)
        {
            return new QueryPage((pageNumber - 1) * pageSize, pageSize);
        }
    }
}
