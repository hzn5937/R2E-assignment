using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
