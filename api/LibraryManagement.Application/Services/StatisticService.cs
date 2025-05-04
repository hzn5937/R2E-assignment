using LibraryManagement.Application.DTOs.Statistic;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                TitleAuthor = $"{book.Title} by {book.Author}: {mostPopularBook.Value}",
                CategoryName = $"{category.Name}: { mostPopularCategory.Value}",
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

            var output = new RequestOverviewOutputDto
            {
                TotalRequestCount = totalRequest,
                PendingRequestCount = totalWaiting,
                ApprovedRequestCount = totalApproved,
                RejectedRequestCount = totalRejected
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
    }
}
