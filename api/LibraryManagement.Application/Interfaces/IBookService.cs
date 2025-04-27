using LibraryManagement.Application.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookService
    {
        Task<PaginatedBookOutputDto> GetAllAsync(int pageNum, int pageSize);
        Task<BookDetailDto?> GetByIdAsync(int id);
        Task<BookDetailDto> CreateAsync(CreateBookDto dto);
        Task<BookDetailDto?> UpdateAsync(int id, UpdateBookDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
