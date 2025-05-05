using Moq;
using LibraryManagement.Api.Controllers;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.DTOs.Statistic;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Test
{
    [TestFixture]
    public class StatisticControllerTests
    {
        private Mock<IStatisticService> _mockStatisticService;
        private StatisticController _statisticController;

        [SetUp]
        public void Setup()
        {
            _mockStatisticService = new Mock<IStatisticService>();
            _statisticController = new StatisticController(_mockStatisticService.Object);
        }

        [Test]
        public async Task GetBookQuantities_ReturnsOkResult_WithBookQuantities()
        {
            // Arrange
            var expectedQuantities = new BookQuantitiesOutputDto { TotalBooks = 100, AvailableBooks = 80, BorrowedBooks = 20 };
            _mockStatisticService.Setup(service => service.GetBookQuantitiesAsync()).ReturnsAsync(expectedQuantities);

            // Act
            var result = await _statisticController.GetBookQuantities();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedQuantities));
            _mockStatisticService.Verify(s => s.GetBookQuantitiesAsync(), Times.Once);
        }

        [Test]
        public async Task GetBooksPerCategory_ReturnsOkResult_WithBooksPerCategory()
        {
            // Arrange
            var expectedBooksPerCategory = new BooksPerCategoryOutputDto
            {
                BooksPerCategory = new List<BooksPerCategory>
                {
                    new BooksPerCategory { CategoryName = "Fiction", BookCount = 50 },
                    new BooksPerCategory { CategoryName = "Science", BookCount = 30 }
                }
            };
            _mockStatisticService.Setup(service => service.GetBooksPerCategoryAsync()).ReturnsAsync(expectedBooksPerCategory);

            // Act
            var result = await _statisticController.GetBooksPerCategory();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedBooksPerCategory));
            _mockStatisticService.Verify(s => s.GetBooksPerCategoryAsync(), Times.Once);
        }

        [Test]
        public async Task GetMostPopular_ReturnsOkResult_WithMostPopularData()
        {
            // Arrange
            var expectedPopular = new MostPopularOutputDto { TitleAuthor = "Test Book by Test Author - 10", CategoryName = "Fiction - 15" };
            _mockStatisticService.Setup(service => service.GetMostPopularAsync()).ReturnsAsync(expectedPopular);

            // Act
            var result = await _statisticController.GetMostPopular();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedPopular));
            _mockStatisticService.Verify(s => s.GetMostPopularAsync(), Times.Once);
        }

        [Test]
        public async Task GetUserCount_ReturnsOkResult_WithUserCounts()
        {
            // Arrange
            var expectedCounts = new UserCountOutputDto { TotalUsers = 95, TotalAdmin = 5 };
            _mockStatisticService.Setup(service => service.GetUserCountAsync()).ReturnsAsync(expectedCounts);

            // Act
            var result = await _statisticController.GetUserCount();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedCounts));
            _mockStatisticService.Verify(s => s.GetUserCountAsync(), Times.Once);
        }

        [Test]
        public async Task GetRequestOverview_WhenDataExists_ReturnsOkResult()
        {
            // Arrange
            var expectedOverview = new RequestOverviewOutputDto
            {
                TotalRequestCount = 50,
                PendingRequestCount = 5,
                ApprovedRequestCount = 30,
                RejectedRequestCount = 5,
                ReturnedRequestCount = 10
            };
            _mockStatisticService.Setup(service => service.GetRequestOverviewAsync()).ReturnsAsync(expectedOverview);

            // Act
            var result = await _statisticController.GetRequestOverview();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedOverview));
            _mockStatisticService.Verify(s => s.GetRequestOverviewAsync(), Times.Once);
        }

        [Test]
        public async Task GetRequestOverview_WhenNoData_ReturnsNotFoundResult()
        {
            // Arrange
            _mockStatisticService.Setup(service => service.GetRequestOverviewAsync()).ReturnsAsync((RequestOverviewOutputDto?)null);

            // Act
            var result = await _statisticController.GetRequestOverview();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult?.Value, Is.EqualTo("No requests found."));
            _mockStatisticService.Verify(s => s.GetRequestOverviewAsync(), Times.Once);
        }

        [Test]
        public async Task GetBookOverview_ReturnsOkResult_WithBookOverview()
        {
            // Arrange
            var expectedOverview = new BookCountOutputDto { TotalBooks = 100, TotalAvailable = 80, TotalNotAvailable = 20 };
            _mockStatisticService.Setup(service => service.GetBookOverviewAsync()).ReturnsAsync(expectedOverview);

            // Act
            var result = await _statisticController.GetBookOverview();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedOverview));
            _mockStatisticService.Verify(s => s.GetBookOverviewAsync(), Times.Once);
        }

        [Test]
        public async Task GetMonthlyReport_WithValidDate_ReturnsOkResult()
        {
            // Arrange
            int year = 2024;
            int month = 4;
            var reportDate = new DateTime(year, month, 1);
            var expectedReport = new MonthlyReportOutputDto { Month = reportDate, TotalRequests = 15 };
            _mockStatisticService.Setup(service => service.GetMonthlyReportAsync(reportDate)).ReturnsAsync(expectedReport);

            // Act
            var result = await _statisticController.GetMonthlyReport(year, month);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedReport));
            _mockStatisticService.Verify(s => s.GetMonthlyReportAsync(reportDate), Times.Once);
        }

        [Test]
        public async Task GetMonthlyReport_WithInvalidDate_ReturnsBadRequest()
        {
            // Arrange
            int year = 2024;
            int invalidMonth = 13; // Invalid month

            // Act
            var result = await _statisticController.GetMonthlyReport(year, invalidMonth);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Invalid date. Year and month must be valid."));
            // Service should not be called with invalid date parts before DateTime creation
            _mockStatisticService.Verify(s => s.GetMonthlyReportAsync(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public async Task GetMonthlyReportsRange_WithValidDateRange_ReturnsOkResult()
        {
            // Arrange
            int startYear = 2024, startMonth = 1;
            int endYear = 2024, endMonth = 3;
            var startDate = new DateTime(startYear, startMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1);
            var expectedReports = new List<MonthlyReportOutputDto>
            {
                new MonthlyReportOutputDto { Month = startDate, TotalRequests = 10 },
                new MonthlyReportOutputDto { Month = startDate.AddMonths(1), TotalRequests = 12 },
                new MonthlyReportOutputDto { Month = endDate, TotalRequests = 15 }
            };
            _mockStatisticService.Setup(service => service.GetMonthlyReportsRangeAsync(startDate, endDate)).ReturnsAsync(expectedReports);

            // Act
            var result = await _statisticController.GetMonthlyReportsRange(startYear, startMonth, endYear, endMonth);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(expectedReports));
            _mockStatisticService.Verify(s => s.GetMonthlyReportsRangeAsync(startDate, endDate), Times.Once);
        }

        [Test]
        public async Task GetMonthlyReportsRange_WithInvalidDateRange_ReturnsBadRequest()
        {
            // Arrange
            int startYear = 2024, startMonth = 13; // Invalid month
            int endYear = 2024, endMonth = 3;

            // Act
            var result = await _statisticController.GetMonthlyReportsRange(startYear, startMonth, endYear, endMonth);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Invalid date range. Year and month must be valid."));
            // Service should not be called with invalid date parts before DateTime creation
            _mockStatisticService.Verify(s => s.GetMonthlyReportsRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public async Task ExportMonthlyReport_WithValidDate_ReturnsFileResult()
        {
            // Arrange
            int year = 2024;
            int month = 4;
            var reportDate = new DateTime(year, month, 1);
            var expectedExcelData = new byte[] { 1, 2, 3 };
            var expectedFileName = $"Monthly_Report_{year}_{month}.xlsx";
            var expectedContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            _mockStatisticService.Setup(service => service.ExportMonthlyReportToExcelAsync(reportDate)).ReturnsAsync(expectedExcelData);

            // Act
            var result = await _statisticController.ExportMonthlyReport(year, month);

            // Assert
            Assert.That(result, Is.InstanceOf<FileContentResult>());
            var fileResult = result as FileContentResult;
            Assert.That(fileResult?.FileContents, Is.EqualTo(expectedExcelData));
            Assert.That(fileResult?.ContentType, Is.EqualTo(expectedContentType));
            Assert.That(fileResult?.FileDownloadName, Is.EqualTo(expectedFileName));
            _mockStatisticService.Verify(s => s.ExportMonthlyReportToExcelAsync(reportDate), Times.Once);
        }

        [Test]
        public async Task ExportMonthlyReport_WithInvalidDate_ReturnsBadRequest()
        {
            // Arrange
            int year = 2024;
            int invalidMonth = 0; // Invalid month

            // Act
            var result = await _statisticController.ExportMonthlyReport(year, invalidMonth);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Invalid date. Year and month must be valid."));
            _mockStatisticService.Verify(s => s.ExportMonthlyReportToExcelAsync(It.IsAny<DateTime>()), Times.Never);
        }

        [Test]
        public async Task ExportMonthlyReportsRange_WithValidDateRange_ReturnsFileResult()
        {
            // Arrange
            int startYear = 2024, startMonth = 1;
            int endYear = 2024, endMonth = 3;
            var startDate = new DateTime(startYear, startMonth, 1);
            var endDate = new DateTime(endYear, endMonth, 1);
            var expectedExcelData = new byte[] { 4, 5, 6, 7 };
            var expectedFileName = $"Monthly_Reports_{startYear}_{startMonth}_to_{endYear}_{endMonth}.xlsx";
            var expectedContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            _mockStatisticService.Setup(service => service.ExportMonthlyReportsRangeToExcelAsync(startDate, endDate)).ReturnsAsync(expectedExcelData);

            // Act
            var result = await _statisticController.ExportMonthlyReportsRange(startYear, startMonth, endYear, endMonth);

            // Assert
            Assert.That(result, Is.InstanceOf<FileContentResult>());
            var fileResult = result as FileContentResult;
            Assert.That(fileResult?.FileContents, Is.EqualTo(expectedExcelData));
            Assert.That(fileResult?.ContentType, Is.EqualTo(expectedContentType));
            Assert.That(fileResult?.FileDownloadName, Is.EqualTo(expectedFileName));
            _mockStatisticService.Verify(s => s.ExportMonthlyReportsRangeToExcelAsync(startDate, endDate), Times.Once);
        }

        [Test]
        public async Task ExportMonthlyReportsRange_WithInvalidDateRange_ReturnsBadRequest()
        {
            // Arrange
            int startYear = 2024, startMonth = 1;
            int endYear = 2023, endMonth = 13; // Invalid end month and end year < start year (DateTime handles this check, but we test the controller entry)

            // Act
            var result = await _statisticController.ExportMonthlyReportsRange(startYear, startMonth, endYear, endMonth);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Invalid date range. Year and month must be valid."));
            _mockStatisticService.Verify(s => s.ExportMonthlyReportsRangeToExcelAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
        }
    }
}