using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.EntityConfiguration
{
    public class BookBorrowingRequestConfiguration : IEntityTypeConfiguration<BookBorrowingRequest>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequest> entity)
        {
            entity.ToTable("BookBorrowingRequests");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.DateRequested)
                  .IsRequired()
                  .HasDefaultValueSql("GETUTCDATE()");

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
