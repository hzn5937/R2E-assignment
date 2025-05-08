using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet("book-quantities")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookQuantities()
        {
            var bookQuantities = await _statisticService.GetBookQuantitiesAsync();
            return Ok(bookQuantities);
        }

        [HttpGet("books-per-category")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBooksPerCategory()
        {
            var booksPerCategory = await _statisticService.GetBooksPerCategoryAsync();
            return Ok(booksPerCategory);
        }

        [HttpGet("most-popular")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMostPopular()
        {
            var mostPopularBooks = await _statisticService.GetMostPopularAsync();
            return Ok(mostPopularBooks);
        }

        [HttpGet("user-count")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserCount()
        {
            var users = await _statisticService.GetUserCountAsync();
            return Ok(users);
        }

        [HttpGet("request-overview")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRequestOverview()
        {
            var requestOverview = await _statisticService.GetRequestOverviewAsync();
            if (requestOverview is null)
            {
                return NotFound("No requests found.");
            }
            return Ok(requestOverview);
        }

        [HttpGet("book-overview")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBookOverview()
        {
            var bookCount = await _statisticService.GetBookOverviewAsync();
            return Ok(bookCount);
        }

        [HttpGet("monthly-report")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                var reportDate = new DateTime(year, month, 1);
                var report = await _statisticService.GetMonthlyReportAsync(reportDate);
                return Ok(report);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid date. Year and month must be valid.");
            }
        }

        [HttpGet("monthly-reports-range")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMonthlyReportsRange(
            [FromQuery] int startYear, [FromQuery] int startMonth, 
            [FromQuery] int endYear, [FromQuery] int endMonth)
        {
            try
            {
                var startDate = new DateTime(startYear, startMonth, 1);
                var endDate = new DateTime(endYear, endMonth, 1);
                var reports = await _statisticService.GetMonthlyReportsRangeAsync(startDate, endDate);
                return Ok(reports);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid date range. Year and month must be valid.");
            }
        }

        [HttpGet("export/monthly-report")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportMonthlyReport([FromQuery] int year, [FromQuery] int month)
        {
            try
            {
                var reportDate = new DateTime(year, month, 1);
                var excelData = await _statisticService.ExportMonthlyReportToExcelAsync(reportDate);
                var reportName = $"Monthly_Report_{year}_{month}.xlsx";
                
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportName);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid date. Year and month must be valid.");
            }
        }

        [HttpGet("export/monthly-reports-range")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportMonthlyReportsRange(
            [FromQuery] int startYear, [FromQuery] int startMonth, 
            [FromQuery] int endYear, [FromQuery] int endMonth)
        {
            try
            {
                var startDate = new DateTime(startYear, startMonth, 1);
                var endDate = new DateTime(endYear, endMonth, 1);
                var excelData = await _statisticService.ExportMonthlyReportsRangeToExcelAsync(startDate, endDate);
                var reportName = $"Monthly_Reports_{startYear}_{startMonth}_to_{endYear}_{endMonth}.xlsx";
                
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportName);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid date range. Year and month must be valid.");
            }
        }
    }
}
