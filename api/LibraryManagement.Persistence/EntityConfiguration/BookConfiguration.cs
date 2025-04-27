using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.EntityConfiguration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> entity)
        {
            entity.ToTable("Books");

            entity.HasKey(b => b.Id);

            entity.Property(b => b.Title)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(b => b.TotalQuantity)
                  .IsRequired();

            entity.Property(b => b.AvailableQuantity)
                  .IsRequired()
                  .IsConcurrencyToken();

            entity.Property(b => b.DeletedAt);

            /* 1-to-many Book ⇋ Category  */
            entity.HasOne(b => b.Category)
                  .WithMany(c => c.Books)
                  .HasForeignKey(b => b.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);

            /* 1-to-many Book ⇋ BorrowingRequestDetail  */
            entity.HasMany(b => b.BorrowingRequestDetails)
                  .WithOne(d => d.Book)
                  .HasForeignKey(d => d.BookId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
