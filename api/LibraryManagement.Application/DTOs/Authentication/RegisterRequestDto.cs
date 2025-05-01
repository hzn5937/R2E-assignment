using LibraryManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Authentication
{
    public class RegisterRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [EmailAddress, MaxLength(256)]
        public string Email { get; set; }
        [Required]
        public UserRole Role { get; set; }
    }
}
