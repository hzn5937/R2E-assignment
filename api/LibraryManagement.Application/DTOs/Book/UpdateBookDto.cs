using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Book
{
    public class UpdateBookDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(1, 1000)]
        public int TotalQuantity { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Author { get; set; }
    }
}
