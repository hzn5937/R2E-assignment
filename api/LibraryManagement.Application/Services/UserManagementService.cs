using LibraryManagement.Application.DTOs.UserManagement;
using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Application.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _hasher = new();

        public UserManagementService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserOutputDto>> GetAllUsersAsync(CancellationToken ct = default)
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToUserDto);
        }

        public async Task<UserOutputDto> GetUserByIdAsync(int id, CancellationToken ct = default)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {id} not found");
            }
            
            return MapToUserDto(user);
        }

        public async Task<UserOutputDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken ct = default)
        {
            var existingUserByUsername = await _userRepository.GetByUsernameAsync(createUserDto.Username, ct);
            if (existingUserByUsername != null)
            {
                throw new ConflictException("Username is already taken");
            }

            var existingUserByEmail = await _userRepository.GetByEmailAsync(createUserDto.Email, ct);
            if (existingUserByEmail != null)
            {
                throw new ConflictException("Email is already registered");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                Role = createUserDto.Role
            };

            user.PasswordHash = _hasher.HashPassword(user, createUserDto.Password);

            await _userRepository.AddAsync(user, ct);

            return MapToUserDto(user);
        }

        public async Task<UserOutputDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto, CancellationToken ct = default)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {id} not found");
            }

            if (updateUserDto.Username != user.Username)
            {
                var existingUser = await _userRepository.GetByUsernameAsync(updateUserDto.Username, ct);
                if (existingUser != null && existingUser.Id != id)
                {
                    throw new ConflictException("Username is already taken");
                }
            }

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;
            user.Role = updateUserDto.Role;

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.PasswordHash = _hasher.HashPassword(user, updateUserDto.Password);
            }

            await _userRepository.UpdateAsync(user, ct);

            return MapToUserDto(user);
        }

        public async Task<bool> DeleteUserAsync(int id, CancellationToken ct = default)
        {
            await _userRepository.DeleteAsync(id, ct);
            return true;
        }

        private static UserOutputDto MapToUserDto(User user)
        {
            return new UserOutputDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}