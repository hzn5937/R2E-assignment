using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookService
    {
        Task<PaginatedOutputDto<UserBookDto>> GetAllAsync(int pageNum=1, int pageSize=5);
        Task<BookDetailDto?> GetByIdAsync(int id);
        Task<BookDetailDto> CreateAsync(CreateBookDto createBookDto);
        Task<BookDetailDto?> UpdateAsync(int id, UpdateBookDto updateBookDto);
        Task<bool> DeleteAsync(int id);
    }
}
