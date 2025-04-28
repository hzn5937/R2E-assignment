using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("Users");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                  .IsRequired()
                  .HasMaxLength(256);

            entity.HasIndex(u => u.Username).IsUnique();

            entity.Property(u => u.Email)
                  .HasMaxLength(256);

            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.PasswordHash)
                  .IsRequired()
                  .HasMaxLength(512);

            entity.Property(u => u.Role)
                  .IsRequired()
                  .HasConversion<string>()          
                  .HasMaxLength(32);

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
