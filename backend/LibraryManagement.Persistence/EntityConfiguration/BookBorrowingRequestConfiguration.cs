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
    public class BookBorrowingRequestConfiguration : IEntityTypeConfiguration<BookBorrowingRequest>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequest> entity)
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.DateRequested).IsRequired();
            entity.Property(r => r.Status).IsRequired();

            entity.HasOne(r => r.Requestor)
                  .WithMany(u => u.RequestsMade)
                  .HasForeignKey(r => r.RequestorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Approver)
                  .WithMany(u => u.RequestsApproved)
                  .HasForeignKey(r => r.ApproverId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(r => r.Details)
                  .WithOne(d => d.Request)
                  .HasForeignKey(d => d.RequestId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
