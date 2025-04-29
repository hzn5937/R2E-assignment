using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository _userRepository;

        public RequestService(IRequestRepository requestRepository, IUserRepository userRepository)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
        }

        public async Task<AvailableRequestOutputDto> GetAvailableRequestsAsync(int userId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            // first day of current month
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var existingRequests = await _requestRepository.GetExistingRequestsOfTheMonth(userId, date);

            var availableRequests = Constants.MonthlyMaximumRequest - existingRequests.Count();

            // idk just in case 
            if (availableRequests < 0)
            {
                availableRequests = 0;
            }

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

        public async Task<RequestOutputDto> GetAllRequestsAsync(int userId, int pageNum = 1, int pageSize = 5)
        {
            return new RequestOutputDto();
        }
        
    }
}
