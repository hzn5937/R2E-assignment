using LibraryManagement.Application.DTOs.Statistic;

namespace LibraryManagement.Application.Interfaces
{
    public interface IStatisticService
    {
        Task<BookQuantitiesOutputDto> GetBookQuantitiesAsync();
        Task<BooksPerCategoryOutputDto> GetBooksPerCategoryAsync();
        Task<MostPopularOutputDto> GetMostPopularAsync();
        Task<UserCountOutputDto> GetUserCountAsync();
        Task<RequestOverviewOutputDto?> GetRequestOverviewAsync();
        Task<BookCountOutputDto> GetBookOverviewAsync();
    }
}
