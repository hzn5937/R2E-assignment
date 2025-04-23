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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasMany(c => c.Books)
                  .WithOne(b => b.Category)
                  .HasForeignKey(b => b.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull);
        }
    }

}
