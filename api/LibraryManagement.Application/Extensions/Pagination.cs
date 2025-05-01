using LibraryManagement.Application.DTOs.Common;

namespace LibraryManagement.Application.Extensions
{
    public static class Pagination
    {
        public static PaginatedOutputDto<T> Paginate<T>(List<T> source, int pageNumber, int pageSize)
            where T : class
        {
            var totalCount = source.Count;
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedOutputDto<T>
            {
                Items = items,
                PageSize = pageSize,
                PageNum = pageNumber,
                TotalPage = (int)Math.Ceiling((double)totalCount / pageSize),
                TotalCount = totalCount,
            };
        }
    }
}
