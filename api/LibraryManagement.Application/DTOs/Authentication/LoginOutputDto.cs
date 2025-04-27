using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Authentication
{
    public class LoginOutputDto
    {
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpires { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
    }
}
    