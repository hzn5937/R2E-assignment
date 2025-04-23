using LibraryManagement.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        public UserRole Role { get; set; } = UserRole.User;
        public ICollection<BookBorrowingRequest> RequestsMade { get; set; } = new List<BookBorrowingRequest>();
        public ICollection<BookBorrowingRequest> RequestsApproved { get; set; } = new List<BookBorrowingRequest>();

    }
}
