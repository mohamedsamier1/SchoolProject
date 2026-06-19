namespace SchoolProject.Core.Wrappers
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(List<T> data)
        {

            Data = data;
        }
        public List<T> Data { get; set; }
        internal PaginatedResult(bool succeeded, List<T> data = default, List<string> message = null, int count = 0, int page = 1, int pagesize = 10)
        {
            Data = data;
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pagesize;
            TotalPages = (int)Math.Ceiling(count / (double)pagesize);
        }
        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pagesize)
        {
            return new(true, data, null, count, page, pagesize);
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public object Meta { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage => CurrentPage < TotalCount;
        public List<string> Message { get; set; } = new();
        public bool Succeeded { get; set; }

    }
}
