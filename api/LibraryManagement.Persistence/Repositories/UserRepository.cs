﻿using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var query = from u in _context.Users
                        select u;

            var result = await query.ToListAsync();

            return result;
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            var query = from u in _context.Users
                        where u.Id == id
                        select u;

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            var query = from u in _context.Users
                        where u.Username == username
                        select u;

            var result = await query.FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var query = from u in _context.Users
                        where u.Email == email
                        select u;

            var result = await query.FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingUser = await _context.Users.FindAsync(new object[] { user.Id }, ct);

                if (existingUser == null)
                {
                    throw new InvalidOperationException($"User with ID {user.Id} not found.");
                }

                existingUser.Username = user.Username;
                existingUser.PasswordHash = user.PasswordHash;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;
                existingUser.RefreshToken = user.RefreshToken;
                existingUser.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;

                _context.Entry(existingUser).State = EntityState.Modified;

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw new TransactionFailedException();
            }
        }

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Users.AddAsync(user, ct);

                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw new TransactionFailedException();
            }
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(ct);

            try
            {
                var user = await _context.Users.FindAsync(new object[] { id }, ct);

                if (user == null)
                {
                    throw new NotFoundException($"User with ID {id} not found.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
            }
            catch (NotFoundException)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw new TransactionFailedException();
            }
        }
    }
}
