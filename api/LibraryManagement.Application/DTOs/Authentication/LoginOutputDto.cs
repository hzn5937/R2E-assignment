using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Application.DTOs.Authentication
{
    public class LoginOutputDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpires { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
    }
}
    