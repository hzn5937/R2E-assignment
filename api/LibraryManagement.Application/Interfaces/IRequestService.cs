using LibraryManagement.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    public interface IRequestService
    {
        Task<AvailableRequestOutputDto> GetAvailableRequestsAsync(int userId);
        Task<RequestDetailOutputDto?> GetRequestDetailByIdAsync(int requestId);
        Task<RequestOutputDto> GetAllRequestsAsync(int userId, int pageNum = 1, int pageSize = 5);
    }
}
