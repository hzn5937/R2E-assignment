using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalQuantity { get; set; }

        [Range(0, int.MaxValue)]
        public int AvailableQuantity { get; set; }

        public DateTime? DeletedAt { get; set; }

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<BookBorrowingRequestDetail> BorrowingRequestDetails { get; set; } = new List<BookBorrowingRequestDetail>();
    }
}
