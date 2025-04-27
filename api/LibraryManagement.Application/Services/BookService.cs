using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Extensions;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<PaginatedBookOutputDto> GetAllAsync(int pageNum=1, int pageSize=5)
        {
            var books = await _bookRepository.GetAllAsync();

            var bookList = new List<UserBookDto>();

            foreach (var book in books)
            {
                if (book.DeletedAt is not null)
                {
                    continue;
                }

                var record = new UserBookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    CategoryName = (book.CategoryId is null) ? "Uncategorized" : book.Category.Name,
                    AvailableQuantity = book.AvailableQuantity,
                };

                bookList.Add(record);
            }

            int totalCount = bookList.Count;
            int totalPage = (int)Math.Ceiling(totalCount / (double)pageSize);
            
            var paginatedBooks = bookList.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();

            var output = new PaginatedBookOutputDto()
            {
                Books = paginatedBooks,
                PageSize = pageSize,
                PageNumber = pageNum,
                TotalPage = totalPage,
                TotalCount = totalCount,
                HasNext = pageNum < totalPage,
                HasPrev = pageNum > 1,
            };

            return output;
        }

        public async Task<BookDetailDto?> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book is null)
            {
                return null;
            }

            var output = new BookDetailDto()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                TotalQuantity = book.TotalQuantity,
                AvailableQuantity = book.AvailableQuantity,
                DeletedAt = book.DeletedAt,
                CategoryName = book.Category.Name,
            };

            return output;
        }

        public async Task<BookDetailDto> CreateAsync(CreateBookDto createBookDto)
        {
            Book? existing = await _bookRepository.GetByTitleAndAuthorAsync(createBookDto.Title, createBookDto.Author);

            if (existing is not null)
            {
                if (existing.DeletedAt is null)
                {
                    throw new ConflictException($"Book with title: {createBookDto.Title} and author: {createBookDto.Author} already exists!");
                }

                existing.DeletedAt = null;

                var updated = await _bookRepository.UpdateAsync(existing);

                var updatedOutput = new BookDetailDto()
                {
                    Id = updated.Id,
                    Title = updated.Title,
                    Author = updated.Author,
                    TotalQuantity = updated.TotalQuantity,
                    AvailableQuantity = updated.AvailableQuantity,
                    CategoryName = (updated.CategoryId is null) ? "Uncategorized" : updated.Category.Name,
                };

                return updatedOutput;
            }

            Category? existingCategory = await _categoryRepository.GetByIdAsync(createBookDto.CategoryId); 

            var book = new Book()
            {
                Title = createBookDto.Title,
                Author = createBookDto.Author,
                TotalQuantity = 1,
                AvailableQuantity = 1,
                CategoryId = (existingCategory is null) ? null : createBookDto.CategoryId
            };

            var created = await _bookRepository.CreateAsync(book);

            var createdOutput = new BookDetailDto()
            {
                Id = created.Id,
                Title = created.Title,
                Author = created.Author,
                TotalQuantity = created.TotalQuantity,
                AvailableQuantity = created.AvailableQuantity,
                CategoryName = (created.CategoryId is null) ? "Uncategorized" : created.Category.Name
            };

            return createdOutput;
        }

        public async Task<BookDetailDto?> UpdateAsync(int id, UpdateBookDto updateBookDto)
        {
            var duplicate = await _bookRepository.GetByTitleAndAuthorAsync(updateBookDto.Title, updateBookDto.Author);

            if (duplicate is not null && duplicate.Id != id)
            {
                throw new ConflictException($"Existing book with Title: {updateBookDto.Title}, Author: {updateBookDto.Author} found!");
            }

            Book? existing = await _bookRepository.GetByIdAsync(id);

            if (existing is null)
            {
                throw new NotFoundException($"There is no book entry with Id: {id}!");
            }

            var newAvailable = existing.AvailableQuantity + (updateBookDto.TotalQuantity - existing.TotalQuantity);

            if (newAvailable < 0)
            {
                var requested = existing.TotalQuantity - existing.AvailableQuantity;
                throw new ConflictException($"Only {existing.AvailableQuantity} copies left in the storage. Minimum value of new total quantity is '{requested}'");
            }

            Category? existingCategory = await _categoryRepository.GetByIdAsync(updateBookDto.CategoryId);

            existing.Title = updateBookDto.Title;
            existing.Author = updateBookDto.Author;
            existing.CategoryId = (existingCategory is null) ? null : updateBookDto.CategoryId;
            existing.TotalQuantity = updateBookDto.TotalQuantity;
            existing.AvailableQuantity = newAvailable;

            var updated = await _bookRepository.UpdateAsync(existing);

            var output = new BookDetailDto()
            {
                Id = updated.Id,
                Title = updated.Title,
                Author = updated.Author,
                TotalQuantity = updated.TotalQuantity,
                AvailableQuantity = updated.AvailableQuantity,
                DeletedAt = updated.DeletedAt,
                CategoryName = (updated.CategoryId is null) ? "Uncategorized" : updated.Category.Name
            };

            return output;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _bookRepository.GetByIdAsync(id);

            if (existing is null)
            {
                return false;
            }

            existing.DeletedAt = DateTime.UtcNow;

            await _bookRepository.DeleteAsync(existing);

            return true;
        }
    }
}
