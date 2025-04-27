using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Book
{
    public class PaginatedBookOutputDto
    {
        public List<UserBookDto> Books { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrev { get; set; }
    }
}
