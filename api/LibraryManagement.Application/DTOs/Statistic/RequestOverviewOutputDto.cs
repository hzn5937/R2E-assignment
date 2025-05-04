using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Statistic
{
    public class RequestOverviewOutputDto
    {
        public int TotalRequestCount { get; set; }
        public int ApprovedRequestCount { get; set; }
        public int PendingRequestCount { get; set; }
        public int RejectedRequestCount { get; set; }
        public int ReturnedRequestCount { get; set; }
    }
}