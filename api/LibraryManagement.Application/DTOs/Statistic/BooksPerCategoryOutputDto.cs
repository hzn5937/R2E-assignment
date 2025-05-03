using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Statistic
{
    public class BooksPerCategoryOutputDto
    {
        public List<BooksPerCategory> BooksPerCategory { get; set; } = new List<BooksPerCategory>();
    }

    public class BooksPerCategory
    {
        public string CategoryName { get; set; }
        public int BookCount { get; set; }
    }
}
