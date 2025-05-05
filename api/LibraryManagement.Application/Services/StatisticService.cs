using LibraryManagement.Application.DTOs.Statistic;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository _userRepository;

        public StatisticService(
            IBookRepository bookRepository, 
            IRequestRepository requestRepository, 
            ICategoryRepository categoryRepository,
            IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _requestRepository = requestRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<BookQuantitiesOutputDto> GetBookQuantitiesAsync()
        {
            var books = await _bookRepository.GetAllAsync();

            var total = 0;
            var available = 0;
            var borrowed = 0;

            foreach (var book in books)
            {
                total += book.TotalQuantity;
                available += book.AvailableQuantity;
                borrowed += book.TotalQuantity - book.AvailableQuantity;
            }

            var output = new BookQuantitiesOutputDto
            {
                TotalBooks = total,
                AvailableBooks = available,
                BorrowedBooks = borrowed,
            };

            return output;
        }

        public async Task<BooksPerCategoryOutputDto> GetBooksPerCategoryAsync()
        {
            var books = await _bookRepository.GetAllAsync();

            var booksPerCategoryList = books
                .Where(b => b.DeletedAt is null)
                .GroupBy(b => b.CategoryId == null
                    ? Constants.NullCategoryName
                    : b.Category.Name)
                .Select(g => new BooksPerCategory
                {
                    CategoryName = g.Key,
                    BookCount = g.Count()
                })
                .ToList();

            var output = new BooksPerCategoryOutputDto()
            {
                BooksPerCategory = booksPerCategoryList
            };

            return output;
        }

        public async Task<MostPopularOutputDto> GetMostPopularAsync()
        {
            var requests = await _requestRepository.GetAllRequestsAsync();

            var bookDict = new Dictionary<int, int>();
            var categoryDict = new Dictionary<int, int>();

            // This is actually just looping through every detail record in the db (count rejected request is intended)
            foreach (var request in requests)
            {
                foreach (var detail in request.Details)
                {
                    if (bookDict.ContainsKey(detail.BookId))
                    {
                        bookDict[detail.BookId]++;
                    }
                    else
                    {
                        bookDict[detail.BookId] = 1;
                    }
                    if (detail.Book.CategoryId != null)
                    {
                        if (categoryDict.ContainsKey((int)detail.Book.CategoryId))
                        {
                            categoryDict[(int)detail.Book.CategoryId]++;
                        }
                        else
                        {
                            categoryDict[(int)detail.Book.CategoryId] = 1;
                        }
                    }
                }
            }

            var mostPopularBook = bookDict
                .OrderByDescending(kvp => kvp.Value)
                .FirstOrDefault();

            var mostPopularCategory = categoryDict
                .OrderByDescending(kvp => kvp.Value)
                .FirstOrDefault();

            var book = await _bookRepository.GetByIdAsync(mostPopularBook.Key);
            var category = await _categoryRepository.GetByIdAsync(mostPopularCategory.Key);

            var output = new MostPopularOutputDto
            {
                // Name - Borrowed Count
                TitleAuthor = $"{book.Title} by {book.Author} - {mostPopularBook.Value}",
                CategoryName = $"{category.Name} - { mostPopularCategory.Value}",
            };

            return output;
        }

        public async Task<UserCountOutputDto> GetUserCountAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var userList = new List<User>();

            foreach (var user in users)
            {
                if (user.Role == UserRole.User)
                {
                    userList.Add(user);
                }
            }

            var output = new UserCountOutputDto
            {
                TotalUsers = userList.Count,
                TotalAdmin = users.Count() - userList.Count,
            };

            return output;
        }

        public async Task<RequestOverviewOutputDto?> GetRequestOverviewAsync()
        {
            var existingRequest = await _requestRepository.GetAllRequestsAsync();

            if (existingRequest is null || !existingRequest.Any())
            {
                return null;
            }

            var totalRequest = existingRequest.Count();

            var totalWaiting = existingRequest.Count(x => x.Status == RequestStatus.Waiting);
            var totalApproved = existingRequest.Count(x => x.Status == RequestStatus.Approved);
            var totalRejected = existingRequest.Count(x => x.Status == RequestStatus.Rejected);
            var totalReturned = existingRequest.Count(x => x.Status == RequestStatus.Returned);

            var output = new RequestOverviewOutputDto
            {
                TotalRequestCount = totalRequest,
                PendingRequestCount = totalWaiting,
                ApprovedRequestCount = totalApproved,
                RejectedRequestCount = totalRejected,
                ReturnedRequestCount = totalReturned
            };

            return output;
        }

        public async Task<BookCountOutputDto> GetBookOverviewAsync()
        {
            var books = await _bookRepository.GetAllAsync();

            var output = new BookCountOutputDto
            {
                TotalBooks = books.Count(b => b.DeletedAt is null),
                TotalAvailable = books.Count(b => b.DeletedAt is null && b.AvailableQuantity > 0),
                TotalNotAvailable = books.Count(b => b.DeletedAt is null && b.AvailableQuantity == 0),
            };

            return output;
        }

        public async Task<MonthlyReportOutputDto> GetMonthlyReportAsync(DateTime month)
        {
            // Normalize the date to the first day of the month
            var startOfMonth = new DateTime(month.Year, month.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);
            
            var allRequests = await _requestRepository.GetAllRequestsAsync();
            
            // Filter requests for the given month
            var monthlyRequests = allRequests
                .Where(r => r.DateRequested >= startOfMonth && r.DateRequested <= endOfMonth)
                .ToList();
            
            if (!monthlyRequests.Any())
            {
                return new MonthlyReportOutputDto 
                { 
                    Month = startOfMonth,
                    TotalRequests = 0,
                    ApprovedRequests = 0,
                    RejectedRequests = 0,
                    PendingRequests = 0,
                    TotalActiveUsers = 0
                };
            }
            
            // Count request statuses
            var approvedCount = monthlyRequests.Count(r => r.Status == RequestStatus.Approved);
            var rejectedCount = monthlyRequests.Count(r => r.Status == RequestStatus.Rejected);
            var pendingCount = monthlyRequests.Count(r => r.Status == RequestStatus.Waiting);
            
            // Get unique users who made requests this month
            var activeUsers = monthlyRequests.Select(r => r.RequestorId).Distinct().Count();
            
            // Calculate popular books
            var bookBorrowCounts = new Dictionary<int, int>();
            var categoryBorrowCounts = new Dictionary<int, int>();
            
            foreach (var request in monthlyRequests)
            {
                foreach (var detail in request.Details)
                {
                    // Count book popularity
                    if (bookBorrowCounts.ContainsKey(detail.BookId))
                        bookBorrowCounts[detail.BookId]++;
                    else
                        bookBorrowCounts[detail.BookId] = 1;
                        
                    // Count category popularity if exists
                    if (detail.Book.CategoryId.HasValue)
                    {
                        var categoryId = detail.Book.CategoryId.Value;
                        if (categoryBorrowCounts.ContainsKey(categoryId))
                            categoryBorrowCounts[categoryId]++;
                        else
                            categoryBorrowCounts[categoryId] = 1;
                    }
                }
            }
            
            // Get top 5 popular books
            var popularBooks = new List<MonthlyPopularBook>();
            foreach (var bookEntry in bookBorrowCounts.OrderByDescending(bc => bc.Value).Take(5))
            {
                var book = await _bookRepository.GetByIdAsync(bookEntry.Key);
                popularBooks.Add(new MonthlyPopularBook
                {
                    BookId = bookEntry.Key,
                    Title = book.Title,
                    Author = book.Author,
                    BorrowCount = bookEntry.Value
                });
            }
            
            // Get top 3 popular categories
            var popularCategories = new List<MonthlyPopularCategory>();
            foreach (var categoryEntry in categoryBorrowCounts.OrderByDescending(cc => cc.Value).Take(3))
            {
                var category = await _categoryRepository.GetByIdAsync(categoryEntry.Key);
                popularCategories.Add(new MonthlyPopularCategory
                {
                    CategoryId = categoryEntry.Key,
                    CategoryName = category.Name,
                    BorrowCount = categoryEntry.Value
                });
            }
            
            // Create and return monthly report
            return new MonthlyReportOutputDto
            {
                Month = startOfMonth,
                TotalRequests = monthlyRequests.Count,
                ApprovedRequests = approvedCount,
                RejectedRequests = rejectedCount,
                PendingRequests = pendingCount,
                PopularBooks = popularBooks,
                PopularCategories = popularCategories,
                TotalActiveUsers = activeUsers
            };
        }

        public async Task<List<MonthlyReportOutputDto>> GetMonthlyReportsRangeAsync(DateTime startMonth, DateTime endMonth)
        {
            // Normalize dates to first day of the month
            var start = new DateTime(startMonth.Year, startMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = new DateTime(endMonth.Year, endMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            
            // Ensure start is before end
            if (start > end)
            {
                var temp = start;
                start = end;
                end = temp;
            }
            
            var reports = new List<MonthlyReportOutputDto>();
            var currentMonth = start;
            
            // Generate reports for each month in the range
            while (currentMonth <= end)
            {
                var report = await GetMonthlyReportAsync(currentMonth);
                reports.Add(report);
                currentMonth = currentMonth.AddMonths(1);
            }
            
            return reports;
        }

        public async Task<byte[]> ExportMonthlyReportToExcelAsync(DateTime month)
        {
            var report = await GetMonthlyReportAsync(month);
            return CreateExcelReport(new List<MonthlyReportOutputDto> { report });
        }

        public async Task<byte[]> ExportMonthlyReportsRangeToExcelAsync(DateTime startMonth, DateTime endMonth)
        {
            var reports = await GetMonthlyReportsRangeAsync(startMonth, endMonth);
            return CreateExcelReport(reports);
        }

        private byte[] CreateExcelReport(List<MonthlyReportOutputDto> reports)
        {
            using (var workbook = new XLWorkbook())
            {
                // Create the Summary worksheet
                var summarySheet = workbook.Worksheets.Add("Summary");
                
                // Add header
                summarySheet.Cell(1, 1).Value = "Month";
                summarySheet.Cell(1, 2).Value = "Total Requests";
                summarySheet.Cell(1, 3).Value = "Approved";
                summarySheet.Cell(1, 4).Value = "Pending";
                summarySheet.Cell(1, 5).Value = "Rejected";
                summarySheet.Cell(1, 6).Value = "Active Users";
                
                // Style the header
                var headerRange = summarySheet.Range(1, 1, 1, 6);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                
                // Add data for each month
                for (int i = 0; i < reports.Count; i++)
                {
                    var row = i + 2; // Start from row 2
                    var report = reports[i];
                    
                    summarySheet.Cell(row, 1).Value = report.Month.ToString("MMMM yyyy");
                    summarySheet.Cell(row, 2).Value = report.TotalRequests;
                    summarySheet.Cell(row, 3).Value = report.ApprovedRequests;
                    summarySheet.Cell(row, 4).Value = report.PendingRequests;
                    summarySheet.Cell(row, 5).Value = report.RejectedRequests;
                    summarySheet.Cell(row, 6).Value = report.TotalActiveUsers;
                }
                
                // Auto fit columns
                summarySheet.Columns().AdjustToContents();
                
                // Create a detailed worksheet for each month
                foreach (var report in reports)
                {
                    var monthName = report.Month.ToString("MMM_yyyy");
                    var detailSheet = workbook.Worksheets.Add(monthName);
                    
                    // Add report title
                    detailSheet.Cell(1, 1).Value = $"Monthly Report for {report.Month.ToString("MMMM yyyy")}";
                    detailSheet.Range(1, 1, 1, 5).Merge();
                    detailSheet.Cell(1, 1).Style.Font.Bold = true;
                    detailSheet.Cell(1, 1).Style.Font.FontSize = 14;
                    detailSheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    
                    // Request summary
                    detailSheet.Cell(3, 1).Value = "Request Summary";
                    detailSheet.Cell(3, 1).Style.Font.Bold = true;
                    
                    detailSheet.Cell(4, 1).Value = "Total Requests:";
                    detailSheet.Cell(4, 2).Value = report.TotalRequests;
                    
                    detailSheet.Cell(5, 1).Value = "Approved Requests:";
                    detailSheet.Cell(5, 2).Value = report.ApprovedRequests;
                    
                    detailSheet.Cell(6, 1).Value = "Pending Requests:";
                    detailSheet.Cell(6, 2).Value = report.PendingRequests;
                    
                    detailSheet.Cell(7, 1).Value = "Rejected Requests:";
                    detailSheet.Cell(7, 2).Value = report.RejectedRequests;
                    
                    detailSheet.Cell(8, 1).Value = "Total Active Users:";
                    detailSheet.Cell(8, 2).Value = report.TotalActiveUsers;
                    
                    // Popular Books section
                    detailSheet.Cell(10, 1).Value = "Most Popular Books";
                    detailSheet.Cell(10, 1).Style.Font.Bold = true;
                    
                    detailSheet.Cell(11, 1).Value = "Title";
                    detailSheet.Cell(11, 2).Value = "Author";
                    detailSheet.Cell(11, 3).Value = "Borrow Count";
                    detailSheet.Range(11, 1, 11, 3).Style.Font.Bold = true;
                    
                    int bookRow = 12;
                    foreach (var book in report.PopularBooks)
                    {
                        detailSheet.Cell(bookRow, 1).Value = book.Title;
                        detailSheet.Cell(bookRow, 2).Value = book.Author;
                        detailSheet.Cell(bookRow, 3).Value = book.BorrowCount;
                        bookRow++;
                    }
                    
                    // Popular Categories section
                    detailSheet.Cell(bookRow + 1, 1).Value = "Most Popular Categories";
                    detailSheet.Cell(bookRow + 1, 1).Style.Font.Bold = true;
                    
                    detailSheet.Cell(bookRow + 2, 1).Value = "Category";
                    detailSheet.Cell(bookRow + 2, 2).Value = "Borrow Count";
                    detailSheet.Range(bookRow + 2, 1, bookRow + 2, 2).Style.Font.Bold = true;
                    
                    int categoryRow = bookRow + 3;
                    foreach (var category in report.PopularCategories)
                    {
                        detailSheet.Cell(categoryRow, 1).Value = category.CategoryName;
                        detailSheet.Cell(categoryRow, 2).Value = category.BorrowCount;
                        categoryRow++;
                    }
                    
                    // Auto fit columns
                    detailSheet.Columns().AdjustToContents();
                }
                
                // Return the Excel workbook as a byte array
                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
