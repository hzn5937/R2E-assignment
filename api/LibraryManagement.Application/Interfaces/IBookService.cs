using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Common;
using LibraryManagement.Domain.Common;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookCountOutputDto> GetBookCountAsync();
        Task<PaginatedOutputDto<UserBookOutputDto>> GetAllAsync(int pageNum = Constants.DefaultPageNum, int pageSize = Constants.DefaultPageSize);
        Task<BookDetailOutputDto?> GetByIdAsync(int id);
        Task<BookDetailOutputDto> CreateAsync(CreateBookDto createBookDto);
        Task<BookDetailOutputDto?> UpdateAsync(int id, UpdateBookDto updateBookDto);
        Task<bool> DeleteAsync(int id);
    }
}
