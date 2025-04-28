using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            var query = from u in _context.Users
                        where u.Username == username
                        select u;

            var result = await query.FirstOrDefaultAsync(ct);

            return result;
        }

        public async Task UpdateAsync(User user, CancellationToken ct = default)
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
        }

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await _context.Users.AddAsync(user, ct);

            await _context.SaveChangesAsync(ct);
        }
    }
}
