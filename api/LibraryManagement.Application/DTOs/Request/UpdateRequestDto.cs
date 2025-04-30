using LibraryManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Request
{
    public class UpdateRequestDto
    {
        [Required]
        public int AdminId { get; set; }
        [Required]
        public RequestStatus Status { get; set; }
    }
}
