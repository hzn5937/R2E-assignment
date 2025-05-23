﻿using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly AppDbContext _context;

        public RequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetExistingRequestsOfTheMonth(int userId, DateTime date)
        {
            var query = from r in _context.BookBorrowingRequests
                        where r.RequestorId == userId
                        && (r.DateRequested.Year == date.Year && r.DateRequested.Month == date.Month)
                        && r.Status != RequestStatus.Rejected
                        select r;

            var existingRequests = await query.ToListAsync();

            return existingRequests;
        }

        public async Task<BookBorrowingRequest?> GetRequestByIdAsync(int requestId)
        {
            var result = await _context.BookBorrowingRequests
                .Include(r => r.Requestor)
                .Include(r => r.Approver)
                .Include(r => r.Details)
                    .ThenInclude(d => d.Book)
                    .ThenInclude(b => b.Category)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            return result;
        }
        public async Task<IEnumerable<BookBorrowingRequest>> GetAllUserRequestsAsync(int userId)
        {
            var result = await _context.BookBorrowingRequests
.Include(r => r.Requestor)
               .Include(r => r.Approver)
               .Include(r => r.Details)
                   .ThenInclude(d => d.Book)
                   .ThenInclude(b => b.Category)
               .Where(r => r.RequestorId == userId)
               .OrderByDescending(r => r.DateRequested)
               .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetAllRequestsAsync()
        {
            var result = await _context.BookBorrowingRequests
               .Include(r => r.Requestor)
               .Include(r => r.Approver)
               .Include(r => r.Details)
                   .ThenInclude(d => d.Book)
                   .ThenInclude(b => b.Category)
               .OrderByDescending(r => r.DateRequested)
               .ToListAsync();

            return result;
        }

        public async Task<BookBorrowingRequest> CreateRequestAsync(BookBorrowingRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.BookBorrowingRequests.Add(request);
                await _context.SaveChangesAsync();

                var created = await _context.BookBorrowingRequests
                    .Include(r => r.Requestor)
                    .Include(r => r.Approver)
                    .Include(r => r.Details)
                        .ThenInclude(d => d.Book)
                        .ThenInclude(b => b.Category)
                    .FirstAsync(r => r.Id == request.Id);

                await transaction.CommitAsync();
                return created;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new TransactionFailedException();
            }
        }

        public async Task<BookBorrowingRequest?> UpdateRequestAsync(BookBorrowingRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingRequest = await _context.BookBorrowingRequests
                    .Include(r => r.Requestor)
                    .Include(r => r.Approver)
                    .Include(r => r.Details)
                        .ThenInclude(d => d.Book)
                        .ThenInclude(b => b.Category)
                    .FirstOrDefaultAsync(r => r.Id == request.Id);

                if (existingRequest != null)
                {
                    _context.Entry(existingRequest).CurrentValues.SetValues(request);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return existingRequest;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new TransactionFailedException();
            }
        }
    }
}
