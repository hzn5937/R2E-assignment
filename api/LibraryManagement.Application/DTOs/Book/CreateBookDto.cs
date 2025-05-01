using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Book
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
