using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Request
{
    public class ReturnBookRequestDto
    {
        [Required]
        public int RequestId { get; set; }
        
        // Optional: The admin ID who processed the return (if returned to an admin)
        public int? ProcessedById { get; set; }
    }
}