using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Request
{
    public class CreateRequestDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one BookId must be provided.")]
        [MaxLength(5, ErrorMessage = "Can only borrow up to 5 books for each request.")]
        public List<int> BookIds { get; set; }
    }
}
