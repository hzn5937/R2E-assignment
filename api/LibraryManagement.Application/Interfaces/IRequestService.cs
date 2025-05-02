using LibraryManagement.Application.DTOs.Common;
using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Domain.Common;

namespace LibraryManagement.Application.Interfaces
{
    public interface IRequestService
    {
        Task<RequestOverviewOutputDto?> GetRequestOverviewAsync();
        Task<AvailableRequestOutputDto> GetAvailableRequestsAsync(int userId);
        Task<RequestDetailOutputDto?> GetRequestDetailByIdAsync(int requestId);
        Task<PaginatedOutputDto<RequestOutputDto>?> GetAllUserRequestsAsync(int userId, int pageNum = Constants.DefaultPageNum, int pageSize = Constants.DefaultPageSize);
        Task<PaginatedOutputDto<RequestDetailOutputDto>?> GetAllRequestDetailsAsync(int pageNum = Constants.DefaultPageNum, int pageSize = Constants.DefaultPageSize);
        Task<RequestDetailOutputDto?> CreateRequestAsync(CreateRequestDto createRequestDto);
        Task<RequestDetailOutputDto?> UpdateRequestAsync(int id, UpdateRequestDto updateRequestDto);
    }
}
