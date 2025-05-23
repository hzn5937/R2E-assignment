﻿using LibraryManagement.Application.DTOs.Request;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/requests")]
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
        [Authorize]
        public async Task<IActionResult> GetNumberOfAvailableRequests(int userId)
        {
            var availableRequest = await _requestService.GetAvailableRequestsAsync(userId);

            return Ok(availableRequest);
        }

        [HttpGet("{requestId}/details")]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> GetAllUserRequests([FromQuery] int userId ,[FromQuery] int pageNum=Constants.DefaultPageNum, [FromQuery] int pageSize=Constants.DefaultPageSize)
        {
            var requests = await _requestService.GetAllUserRequestsAsync(userId, pageNum, pageSize);

            if (requests is null)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            return Ok(requests);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRequests([FromQuery] string? status, [FromQuery] int pageNum = Constants.DefaultPageNum, [FromQuery] int pageSize = Constants.DefaultPageSize)
        {
            var requestDetails = await _requestService.GetAllRequestDetailsAsync(status, pageNum, pageSize);

            if (requestDetails is null)
            {
                return NotFound("Failed to load request details.");
            }

            if (requestDetails.TotalCount <= 0)
            {
                return NotFound($"No requests found with status: {status}");
            }

            return Ok(requestDetails);
        }

        [HttpPost("create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateRequest(CreateRequestDto createRequestDto)
        {
            var createdRequest = await _requestService.CreateRequestAsync(createRequestDto);

            if (createdRequest is null)
            {
                return StatusCode(429, $"User with ID {createRequestDto.UserId} has reached the maximum number of requests for this month!");
            }

            return Ok(createdRequest);
        }

        [HttpPut("{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRequest(int requestId, UpdateRequestDto updateRequestDto)
        {
            if (updateRequestDto.Status == RequestStatus.Waiting)
            {
                return StatusCode(400, $"You are not suppose to update any request to Waiting state.");
            }

            var updatedRequest = await _requestService.UpdateRequestAsync(requestId, updateRequestDto);

            if (updatedRequest is null)
            {
                return StatusCode(401, $"{updateRequestDto.AdminId} doesn't have permission to update this request!");
            }

            return Ok(updatedRequest);
        }

        [HttpPost("{requestId}/return")]
        [Authorize]
        public async Task<IActionResult> ReturnBooks(int requestId, [FromBody] ReturnBookRequestDto returnBookRequestDto)
        {
            // Ensure the request ID in path matches the one in the body
            if (requestId != returnBookRequestDto.RequestId)
            {
                return BadRequest("Request ID in the URL does not match the request ID in the body.");
            }

            var returnedRequest = await _requestService.ReturnBooksAsync(returnBookRequestDto);

            if (returnedRequest is null)
            {
                return NotFound($"Failed to process return for request with ID {requestId}.");
            }

            return Ok(returnedRequest);
        }
    }
}
