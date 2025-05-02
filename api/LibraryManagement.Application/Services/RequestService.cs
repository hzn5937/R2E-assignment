using LibraryManagement.Application.DTOs.Common;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces;

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
                Title = detail.Book.Title,
                Author = detail.Book.Author,
                CategoryName = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name
            }).ToList();

            var output = new RequestDetailOutputDto
            {
                Id = existing.Id,
                Books = bookInformationList,
                Requestor = existing.Requestor.Username,
                Approver = existing.Approver?.Username,
                Status = existing.Status.ToString(),
                RequestedDate = existing.DateRequested,
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

        public async Task<PaginatedOutputDto<RequestDetailOutputDto>?> GetAllRequestDetailsAsync(int pageNum = Constants.DefaultPageNum, int pageSize = Constants.DefaultPageSize)
        {
            var allRequests = await _requestRepository.GetAllRequestsAsync();

            var requestList = new List<RequestDetailOutputDto>();

            foreach (var request in allRequests)
            {
                var record = new RequestDetailOutputDto
                {
                    Id = request.Id,
                    Books = request.Details.Select(detail => new BookInformation
                    {
                        Title = detail.Book.Title,
                        Author = detail.Book.Author,
                        CategoryName = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name
                    }).ToList(),
                    Requestor = request.Requestor.Username,
                    Approver = request.Approver?.Username,
                    Status = request.Status.ToString(),
                    RequestedDate = request.DateRequested,
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
                    Title = detail.Book.Title,
                    Author = detail.Book.Author,
                    CategoryName = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name
                }).ToList(),
                Requestor = created.Requestor.Username,
                Status = created.Status.ToString(),
                RequestedDate = created.DateRequested,
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
                    Title = detail.Book.Title,
                    Author = detail.Book.Author,
                    CategoryName = (detail.Book.Category is null) ? Constants.NullCategoryName : detail.Book.Category.Name
                }).ToList(),
                Requestor = updatedRequest.Requestor.Username,
                Approver = updatedRequest.Approver.Username, // it shouldn't be null here
                Status = updatedRequest.Status.ToString(),
                RequestedDate = updatedRequest.DateRequested,
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
