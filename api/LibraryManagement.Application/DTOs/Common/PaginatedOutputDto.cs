namespace LibraryManagement.Application.DTOs.Common
{
    public class PaginatedOutputDto<T>
        where T : class
    {
        public List<T> Items { get; set; }
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext => PageNum < TotalPage;
        public bool HasPrev => PageNum > 1;
    }
}
