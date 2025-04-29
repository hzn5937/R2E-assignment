using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        [Route("available/{userId}")]
        public async Task<IActionResult> GetAvailableRequests(int userId)
        {
            var availableRequest = await _requestService.GetAvailableRequestsAsync(userId);

            return Ok(availableRequest);
        }

        [HttpGet("{requestId}/details")]
        public async Task<IActionResult> GetRequestDetailById(int requestId)
        {
            var requestDetail = await _requestService.GetRequestDetailByIdAsync(requestId);

            if (requestDetail is null)
            {
                return NotFound($"Request with ID {requestId} not found.");
            }

            return Ok(requestDetail);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequest([FromQuery] int userId ,[FromQuery] int pageNum = 1, [FromQuery] int pageSize = 5)
        {
            var requests = await _requestService.GetAllRequestsAsync(userId, pageNum, pageSize);

            if (requests is null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return Ok(requests);
        }

        //[HttpPost("{userId}")]
        //public async Task <IActionResult> CreateRequest(int userId, [FromBody] CreateRequestDto createRequestDto)
        //{
        //    var createdRequest = await _requestService.CreateRequestAsync(userId, createRequestDto);
        //    return Ok(createdRequest);
        //}



    }
}
