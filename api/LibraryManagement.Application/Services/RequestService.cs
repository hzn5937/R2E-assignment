using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.DTOs.Common;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.DTOs.Statistic;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces;
using System.Globalization;

namespace LibraryManagement.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public RequestService(IRequestRepository requestRepository, IUserRepository userRepository, IBookRepository bookRepository)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        public async Task<AvailableRequestOutputDto> GetAvailableRequestsAsync(int userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            var availableRequests = await GetRemainingMonthlyRequestsAsync(userId);

            var output = new AvailableRequestOutputDto
            {
                AvailableRequests = availableRequests
            };

            return output;
        }

        public async Task<RequestDetailOutputDto?> GetRequestDetailByIdAsync(int requestId)
        {
            BookBorrowingRequest? existing = await _requestRepository.GetRequestByIdAsync(requestId);

            if (existing == null)
            {
                return null;
            }
            
            List<BookInformation> bookInformationList = existing.Details.Select(detail => new BookInformation
            {
                Id = detail.Book.Id,
                Title = detail.Book.Title,
                Author = detail.Book.Author,
                CategoryName = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name,
                Category = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name
            }).ToList();

            var output = new RequestDetailOutputDto
            {
                Id = existing.Id,
                Books = bookInformationList,
                Requestor = existing.Requestor.Username,
                Approver = existing.Approver?.Username,
                Status = existing.Status.ToString(),
                RequestedDate = existing.DateRequested,
                DateRequested = existing.DateRequested.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                DateReturned = existing.DateReturned?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            };

            return output;
        }

        public async Task<PaginatedOutputDto<RequestOutputDto>?> GetAllUserRequestsAsync(int userId, int pageNum=Constants.DefaultPageNum, int pageSize=Constants.DefaultPageSize)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);

            if (existingUser is null)
            {
                return null;
            }

            var userRequests = await _requestRepository.GetAllUserRequestsAsync(userId);

            var requestList = new List<RequestOutputDto>();

            foreach (var request in userRequests)
            {
                var record = new RequestOutputDto
                {
                    Id = request.Id,
                    Requestor = request.Requestor.Username,
                    Approver = request.Approver?.Username,
                    Status = request.Status.ToString(),
                    RequestedDate = request.DateRequested,
                };
                requestList.Add(record);
            }

            var paginated = Pagination.Paginate<RequestOutputDto>(requestList, pageNum, pageSize);

            return paginated;
        }

        public async Task<PaginatedOutputDto<RequestDetailOutputDto>?> GetAllRequestDetailsAsync(string? status, int pageNum = Constants.DefaultPageNum, int pageSize = Constants.DefaultPageSize)
        {
            var allRequests = await _requestRepository.GetAllRequestsAsync();

            if (status is not null)
            {
                string processedName = CultureInfo.CurrentCulture.TextInfo
                    .ToTitleCase(status.ToLowerInvariant());

                if (Enum.IsDefined(typeof(RequestStatus), processedName))
                {
                    allRequests = allRequests.Where(x => x.Status.ToString() == processedName).ToList();
                }
                else
                {
                    throw new NotFoundException($"Request status {processedName} not found.");
                }
            }

            var requestList = new List<RequestDetailOutputDto>();

            foreach (var request in allRequests)
            {
                var record = new RequestDetailOutputDto
                {
                    Id = request.Id,
                    Books = request.Details.Select(detail => new BookInformation
                    {
                        Id = detail.Book.Id,
                        Title = detail.Book.Title,
                        Author = detail.Book.Author,
                        CategoryName = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name,
                        Category = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name
                    }).ToList(),
                    Requestor = request.Requestor.Username,
                    Approver = request.Approver?.Username,
                    Status = request.Status.ToString(),
                    RequestedDate = request.DateRequested,
                    DateRequested = request.DateRequested.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    DateReturned = request.DateReturned?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                };

                requestList.Add(record);
            }

            var paginated = Pagination.Paginate<RequestDetailOutputDto>(requestList, pageNum, pageSize);

            return paginated;
        }

        public async Task<RequestDetailOutputDto?> CreateRequestAsync(CreateRequestDto createRequestDto)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(createRequestDto.UserId);

            if (existingUser is null)
            {
                throw new NotFoundException($"User with ID {createRequestDto.UserId} not found.");
            }

            if (await GetRemainingMonthlyRequestsAsync(createRequestDto.UserId) <= 0)
            {
                return null;
            }

            var validBooks = new List<Book>();

            foreach (var bookId in createRequestDto.BookIds)
            {
                var existingBook = await _bookRepository.GetByIdAsync(bookId);
                if (existingBook is null)
                {
                    throw new NotFoundException($"Book with ID {bookId} not found.");
                }

                if (existingBook.AvailableQuantity <= 0)
                {
                    throw new ConflictException($"Book {existingBook.Title} is out of stock.");
                }

                validBooks.Add(existingBook);
            }

            // if deduct from the previous loop, some will be deduct even if the request fail later on.
            foreach (var book in validBooks)
            {
                book.AvailableQuantity--;
                await _bookRepository.UpdateAsync(book);
            }

            var request = new BookBorrowingRequest
            {
                Requestor = existingUser,
                DateRequested = DateTime.UtcNow,
                Status = RequestStatus.Waiting,
                Details = createRequestDto.BookIds.Select(bookId => new BookBorrowingRequestDetail
                {
                    BookId = bookId
                }).ToList()
            };

            var created = await _requestRepository.CreateRequestAsync(request);

            var output = new RequestDetailOutputDto()
            {
                Id = created.Id,
                Books = created.Details.Select(detail => new BookInformation
                {
                    Id = detail.Book.Id,
                    Title = detail.Book.Title,
                    Author = detail.Book.Author,
                    CategoryName = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name,
                    Category = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name
                }).ToList(),
                Requestor = created.Requestor.Username,
                Status = created.Status.ToString(),
                RequestedDate = created.DateRequested,
                DateRequested = created.DateRequested.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            };

            return output;
        }

        public async Task<RequestDetailOutputDto?> UpdateRequestAsync(int id, UpdateRequestDto updateRequestDto)
        {
            var existingRequest = await _requestRepository.GetRequestByIdAsync(id);
            if (existingRequest is null)
            {
                throw new NotFoundException($"Request with ID {id} not found.");
            }
            else if (existingRequest.Status != RequestStatus.Waiting)
            {
                throw new ConflictException($"Request with ID {id} is already {existingRequest.Status}. You should not update any request that have been processed.");
            }

            var existingUser = await _userRepository.GetUserByIdAsync(updateRequestDto.AdminId);
            if (existingUser is null)
            {
                throw new NotFoundException($"User with ID {updateRequestDto.AdminId} not found.");
            }
            else if (existingUser.Role != UserRole.Admin)
            {
                return null;
            }

            existingRequest.ApproverId = updateRequestDto.AdminId;
            existingRequest.Status = updateRequestDto.Status;

            BookBorrowingRequest? updatedRequest = await _requestRepository.UpdateRequestAsync(existingRequest);

            if (updatedRequest is null)
            {
                throw new NotFoundException($"Request with ID {id} not found.");
            }

            if (updateRequestDto.Status == RequestStatus.Rejected)
            {
                updatedRequest.Details.Select(detail => detail.Book).ToList().ForEach(book =>
                {
                    book.AvailableQuantity++;

                    // This could happen because of function testing in earlier phase of development and data seeding
                    if (book.AvailableQuantity > book.TotalQuantity)
                    {
                        book.AvailableQuantity = book.TotalQuantity;
                    }

                    _bookRepository.UpdateAsync(book);
                });
            }

            var output = new RequestDetailOutputDto()
            {
                Id = updatedRequest.Id,
                Books = updatedRequest.Details.Select(detail => new BookInformation
                {
                    Id = detail.Book.Id,
                    Title = detail.Book.Title,
                    Author = detail.Book.Author,
                    CategoryName = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name,
                    Category = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name
                }).ToList(),
                Requestor = updatedRequest.Requestor.Username,
                Approver = updatedRequest.Approver.Username, // it shouldn't be null here
                Status = updatedRequest.Status.ToString(),
                RequestedDate = updatedRequest.DateRequested,
                DateRequested = updatedRequest.DateRequested.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            };

            return output;
        }

        public async Task<RequestDetailOutputDto?> ReturnBooksAsync(ReturnBookRequestDto returnBookRequestDto)
        {
            var existingRequest = await _requestRepository.GetRequestByIdAsync(returnBookRequestDto.RequestId);
            
            if (existingRequest is null)
            {
                throw new NotFoundException($"Request with ID {returnBookRequestDto.RequestId} not found.");
            }
            
            if (existingRequest.Status != RequestStatus.Approved)
            {
                throw new ConflictException($"Request with ID {returnBookRequestDto.RequestId} is {existingRequest.Status}. Only approved requests can be returned.");
            }
            
            if (existingRequest.DateReturned != null)
            {
                throw new ConflictException($"Request with ID {returnBookRequestDto.RequestId} has already been returned on {existingRequest.DateReturned}.");
            }

            // Process the return
            existingRequest.Status = RequestStatus.Returned;
            existingRequest.DateReturned = DateTime.UtcNow;
            
            // If there's a user processing the return, validate the user
            if (returnBookRequestDto.ProcessedById.HasValue)
            {
                var user = await _userRepository.GetUserByIdAsync(returnBookRequestDto.ProcessedById.Value);
                if (user is null)
                {
                    throw new NotFoundException($"User with ID {returnBookRequestDto.ProcessedById.Value} not found.");
                }
                
                // Allow both the requestor and admins to process returns
                bool isRequestor = user.Id == existingRequest.RequestorId;
                bool isAdmin = user.Role == UserRole.Admin;
                
                if (!isRequestor && !isAdmin)
                {
                    throw new ConflictException($"User with ID {returnBookRequestDto.ProcessedById.Value} is not authorized to return this book.");
                }
                
                // Only update the approver ID if it's an admin and not already set
                if (isAdmin && !existingRequest.ApproverId.HasValue)
                {
                    existingRequest.ApproverId = returnBookRequestDto.ProcessedById.Value;
                }
            }

            // Increment available quantity for each book
            foreach (var detail in existingRequest.Details)
            {
                detail.Book.AvailableQuantity++;
                
                // Safety check to make sure we don't exceed total quantity
                if (detail.Book.AvailableQuantity > detail.Book.TotalQuantity)
                {
                    detail.Book.AvailableQuantity = detail.Book.TotalQuantity;
                }
                
                await _bookRepository.UpdateAsync(detail.Book);
            }

            var updatedRequest = await _requestRepository.UpdateRequestAsync(existingRequest);
            
            if (updatedRequest is null)
            {
                throw new NotFoundException($"Failed to update request with ID {returnBookRequestDto.RequestId}.");
            }

            // Map to DTOs for output
            var bookInfoList = updatedRequest.Details.Select(detail => new BookInformation
            {
                Id = detail.Book.Id,
                Title = detail.Book.Title,
                Author = detail.Book.Author,
                CategoryName = detail.Book.Category?.Name ?? Constants.NullCategoryName,
                Category = detail.Book.Category?.Name ?? Constants.NullCategoryName
            }).ToList();

            var output = new RequestDetailOutputDto
            {
                Id = updatedRequest.Id,
                Requestor = updatedRequest.Requestor.Username,
                Approver = updatedRequest.Approver?.Username,
                Status = updatedRequest.Status.ToString(),
                RequestedDate = updatedRequest.DateRequested,
                DateRequested = updatedRequest.DateRequested.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                DateReturned = updatedRequest.DateReturned?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                Books = bookInfoList
            };

            return output;
        }

        // Helper function
        public async Task<int> GetRemainingMonthlyRequestsAsync(int userId)
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var existingRequests = await _requestRepository.GetExistingRequestsOfTheMonth(userId, date);

            var availableRequests = Constants.MonthlyMaximumRequest - existingRequests.Count();

            // idk just in case 
            if (availableRequests < 0)
            {
                availableRequests = 0;
            }

            return availableRequests;
        }

    }
}
