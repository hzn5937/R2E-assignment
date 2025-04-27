using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
