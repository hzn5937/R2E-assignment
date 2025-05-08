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
        Task<MonthlyReportOutputDto> GetMonthlyReportAsync(DateTime month);
        Task<List<MonthlyReportOutputDto>> GetMonthlyReportsRangeAsync(DateTime startMonth, DateTime endMonth);
        Task<byte[]> ExportMonthlyReportToExcelAsync(DateTime month);
        Task<byte[]> ExportMonthlyReportsRangeToExcelAsync(DateTime startMonth, DateTime endMonth);
    }
}
