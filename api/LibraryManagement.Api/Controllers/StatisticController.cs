using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    }
}
