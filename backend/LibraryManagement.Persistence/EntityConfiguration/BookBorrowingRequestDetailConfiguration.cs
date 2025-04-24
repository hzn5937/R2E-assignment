using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Persistence.EntityConfiguration
{
    public class BookBorrowingRequestDetailConfiguration : IEntityTypeConfiguration<BookBorrowingRequestDetail>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequestDetail> entity)
        {
            entity.HasKey(d => d.Id);

            entity.HasOne(d => d.Book)
                  .WithMany(b => b.BorrowingRequestDetails)
                  .HasForeignKey(d => d.BookId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Request)
                  .WithMany(r => r.Details)
                  .HasForeignKey(d => d.RequestId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(d => new { d.RequestId, d.BookId }).IsUnique();
        }
    }
}
