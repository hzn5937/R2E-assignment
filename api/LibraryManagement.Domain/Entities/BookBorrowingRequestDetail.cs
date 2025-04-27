using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class BookBorrowingRequestDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Request")]
        public int RequestId { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }
        public BookBorrowingRequest Request { get; set; }
    }
}
