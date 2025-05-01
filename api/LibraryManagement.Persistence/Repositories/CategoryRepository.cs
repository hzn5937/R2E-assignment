using LibraryManagement.Application.Extensions.Exceptions;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var query = from c in _context.Categories
                        select new Category()
                        {
                            Id = c.Id,
                            Name = c.Name,
                        };

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var query = from c in _context.Categories
                        where c.Id == id
                        select new Category()
                        {
                            Id = c.Id,
                            Name = c.Name,
                        };

            var output = await query.FirstOrDefaultAsync();

            return output;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result.Entity;
            }
            catch 
            {
                await transaction.RollbackAsync();
                throw new TransactionFailedException();
            }
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existing = await _context.Categories.FindAsync(category.Id);

                existing.Name = category.Name;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return existing;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new TransactionFailedException();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var query = from c in _context.Categories
                            where c.Id == id
                            select c;

                var category = await query.FirstAsync();

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new TransactionFailedException();
            }
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            string nameLower = name.ToLower();

            var query = from c in _context.Categories
                        where c.Name.ToLower() == nameLower
                        select new Category()
                        {
                            Id = c.Id,
                            Name = c.Name,
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }
    }
}
