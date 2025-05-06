using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.UserManagement
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
