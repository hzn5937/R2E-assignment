using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities
{
    public class BookBorrowingRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Requestor")]
        public int RequestorId { get; set; }

        [ForeignKey("Approver")]
        public int? ApproverId { get; set; }

        [Required]
        public DateTime DateRequested { get; set; }

        [Required]
        public RequestStatus Status { get; set; }

        public User Requestor { get; set; }
        public User? Approver { get; set; }

        public ICollection<BookBorrowingRequestDetail> Details { get; set; } = new List<BookBorrowingRequestDetail>();
    }
}
