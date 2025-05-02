using LibraryManagement.Application.DTOs.User;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserCountOutputDto> GetUserCountAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var userList = new List<User>();

            foreach (var user in users)
            {
                if (user.Role == UserRole.User)
                {
                    userList.Add(user);
                }
            }

            var output = new UserCountOutputDto
            {
                TotalUsers = userList.Count,
                TotalAdmin = users.Count() - userList.Count,
            };

            return output;
        }
    }
}
