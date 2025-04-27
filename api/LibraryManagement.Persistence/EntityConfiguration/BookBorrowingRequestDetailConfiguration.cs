using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.EntityConfiguration
{
    public class BookBorrowingRequestDetailConfiguration : IEntityTypeConfiguration<BookBorrowingRequestDetail>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequestDetail> entity)
        {
            entity.ToTable("BookBorrowingRequestDetails");

            entity.HasKey(d => d.Id);

            entity.HasOne(d => d.Book)
                  .WithMany(b => b.BorrowingRequestDetails)
                  .HasForeignKey(d => d.BookId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Request)
                  .WithMany(r => r.Details)
                  .HasForeignKey(d => d.RequestId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(d => new { d.RequestId, d.BookId })
                  .IsUnique();
        }
    }
}
