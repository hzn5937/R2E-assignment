using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace LibraryManagement.Persistence.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users");

            entity.Property(u => u.Role).IsRequired();

            entity.HasMany(u => u.RequestsMade)
                  .WithOne(r => r.Requestor)
                  .HasForeignKey(r => r.RequestorId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(u => u.RequestsApproved)
                  .WithOne(r => r.Approver)
                  .HasForeignKey(r => r.ApproverId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
