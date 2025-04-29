using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<BookBorrowingRequest>> GetExistingRequestsOfTheMonth(int userId, DateTime date);
        Task<BookBorrowingRequest?> GetRequestByIdAsync(int requestId);
    }
}
