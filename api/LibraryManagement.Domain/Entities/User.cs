using LibraryManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Username { get; set; } = default!;

        [Required, MaxLength(512)]
        public string PasswordHash { get; set; } = default!;

        [EmailAddress, MaxLength(256)]
        public string? Email { get; set; }

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public ICollection<BookBorrowingRequest> RequestsMade { get; set; } = new List<BookBorrowingRequest>();
        public ICollection<BookBorrowingRequest> RequestsApproved { get; set; } = new List<BookBorrowingRequest>();
    }
}
