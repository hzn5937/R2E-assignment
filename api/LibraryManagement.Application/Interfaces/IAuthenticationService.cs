using LibraryManagement.Application.DTOs.Authentication;
using LibraryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginOutputDto?> VerifyUserAsync(LoginDto dto, CancellationToken ct = default);
        Task<LoginOutputDto?> RefreshAsync(RefreshRequestDto dto, CancellationToken ct = default);
        Task<LoginOutputDto?> RegisterAsync(RegisterRequestDto dto, CancellationToken ct = default);
    }
}
