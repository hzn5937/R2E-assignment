using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
