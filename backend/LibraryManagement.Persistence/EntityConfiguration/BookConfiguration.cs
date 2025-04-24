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
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> entity)
        {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Title)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(b => b.TotalQuantity).IsRequired();
            entity.Property(b => b.AvailableQuantity).IsRequired();

            entity.HasOne(b => b.Category)
                  .WithMany(c => c.Books)
                  .HasForeignKey(b => b.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(b => b.BorrowingRequestDetails)
                  .WithOne(d => d.Book)
                  .HasForeignKey(d => d.BookId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
