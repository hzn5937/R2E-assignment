using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Authentication
{
    public class RefreshRequestDto
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
