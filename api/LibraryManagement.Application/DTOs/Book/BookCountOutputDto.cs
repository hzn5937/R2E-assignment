using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Book
{
    public class BookCountOutputDto
    {
        public int TotalBooks { get; set; }
        public int TotalAvailable { get; set; }
        public int TotalNotAvailable { get; set; }
    }
}
