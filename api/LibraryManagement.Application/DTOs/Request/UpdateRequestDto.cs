using LibraryManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

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
