using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.UserManagement
{
    public class UserOutputDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}