namespace AGOC.Services
{
    public class Pagination<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public int FirstItemIndex => (PageNumber - 1) * PageSize + 1;
        public int LastItemIndex => Math.Min(PageNumber * PageSize, TotalCount);

        // Constructor with parameters for items, total count, page number, and page size
        public Pagination(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        // Default constructor for empty pagination
        public Pagination()
        {
            Items = new List<T>();
            TotalCount = 0;
            PageNumber = 1;
            PageSize = 10;
        }
    }

}