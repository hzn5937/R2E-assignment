using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Persistence.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            var query = from b in _context.Books
                        //orderby b.Title
                        select new Book
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            TotalQuantity = b.TotalQuantity,
                            AvailableQuantity = b.AvailableQuantity,
                            DeletedAt = b.DeletedAt,
                            CategoryId = b.CategoryId,
                            Category = b.Category,
                            BorrowingRequestDetails = b.BorrowingRequestDetails
                        };

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            var query = from b in _context.Books
                        where b.Id == id
                        select new Book
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            TotalQuantity = b.TotalQuantity,
                            AvailableQuantity = b.AvailableQuantity,
                            DeletedAt = b.DeletedAt,
                            CategoryId = b.CategoryId,
                            Category = b.Category,
                            BorrowingRequestDetails = b.BorrowingRequestDetails
                        };

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<Book> CreateAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            var existing = await _context.Books.FindAsync(book.Id);

            existing.Title = book.Title;
            existing.Author = book.Author;
            existing.TotalQuantity = book.TotalQuantity;
            existing.AvailableQuantity = book.AvailableQuantity;
            existing.DeletedAt = book.DeletedAt;
            existing.CategoryId = book.CategoryId;

            await _context.SaveChangesAsync();

            await _context.Entry(existing).Reference(b => b.Category).LoadAsync();

            return existing;
        }

        public async Task DeleteAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetByTitleAndAuthorAsync(string title, string author)
        {
            var lowerTitle = title.ToLower();
            var lowerAuthor = author.ToLower();

            var query = from b in _context.Books
                        where b.Title.ToLower() == lowerTitle && b.Author.ToLower() == lowerAuthor
                        select new Book
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            TotalQuantity = b.TotalQuantity,
                            AvailableQuantity = b.AvailableQuantity,
                            DeletedAt = b.DeletedAt,
                            CategoryId = b.CategoryId,
                            Category = b.Category,
                            BorrowingRequestDetails = b.BorrowingRequestDetails
                        };

            var result = await query.FirstOrDefaultAsync();

            return result;
        }
    }
}
