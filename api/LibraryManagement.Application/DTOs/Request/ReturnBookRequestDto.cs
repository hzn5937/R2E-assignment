using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Request
{
    public class ReturnBookRequestDto
    {
        [Required]
        public int RequestId { get; set; }
        // it is UserId of the user who borrowed the book (I am out of time to fix)
        public int? ProcessedById { get; set; }
    }
}