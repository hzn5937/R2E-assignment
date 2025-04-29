using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Request
{
    public class RequestDetailOutputDto
    {
        public int Id { get; set; }
        public List<BookInformation> Books { get; set; }
        public string Requestor { get; set; }
        public string? Approver { get; set; }
        public string Status { get; set; }
        public DateTime RequestedDate { get; set; }
    }

    public class BookInformation
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string CategoryName { get; set; }
    }
}
