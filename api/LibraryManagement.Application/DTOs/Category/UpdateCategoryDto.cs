using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
