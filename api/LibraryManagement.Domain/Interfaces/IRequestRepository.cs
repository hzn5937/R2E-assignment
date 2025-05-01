using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<BookBorrowingRequest>> GetExistingRequestsOfTheMonth(int userId, DateTime date);
        Task<BookBorrowingRequest?> GetRequestByIdAsync(int requestId);
        Task<IEnumerable<BookBorrowingRequest>> GetAllUserRequestsAsync(int userId);
        Task<IEnumerable<BookBorrowingRequest>> GetAllRequestsAsync();
        Task<BookBorrowingRequest> CreateRequestAsync(BookBorrowingRequest request);
        Task<BookBorrowingRequest?> UpdateRequestAsync(BookBorrowingRequest request);
    }
}
