using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Request
{
    public class RequestOutputDto
    {
        public int Id { get; set; }
        public string Requestor { get; set; }
        public string? Aprrover { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }
    }
}
