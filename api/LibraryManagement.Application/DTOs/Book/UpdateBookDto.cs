using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Book
{
    public class UpdateBookDto
    {
        public string Title { get; set; }
        public int TotalQuantity { get; set; }
        public int CategoryId { get; set; }
        public string Author { get; set; }
    }
}
