using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Statistic
{
    public class UserCountOutputDto
    {
        public int TotalUsers { get; set; }
        public int TotalAdmin { get; set; }
    }
}