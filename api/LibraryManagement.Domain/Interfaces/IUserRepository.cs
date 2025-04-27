using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task UpdateAsync(User user, CancellationToken ct = default);
        Task AddAsync(User user, CancellationToken ct = default);
    }
}
