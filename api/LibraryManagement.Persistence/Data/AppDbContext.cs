using LibraryManagement.Domain.Entities;
using LibraryManagement.Persistence.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<BookBorrowingRequest> BookBorrowingRequests => Set<BookBorrowingRequest>();
        public DbSet<BookBorrowingRequestDetail> BookBorrowingRequestDetails => Set<BookBorrowingRequestDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new BookBorrowingRequestConfiguration());
            modelBuilder.ApplyConfiguration(new BookBorrowingRequestDetailConfiguration());

            ModelBuilderSeeder.Seed(modelBuilder);
        }
    }
}
