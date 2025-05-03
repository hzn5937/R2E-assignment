using LibraryManagement.Application.DTOs.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    public interface IStatisticService
    {
        Task<BookQuantitiesOutputDto> GetBookQuantitiesAsync();
        Task<BooksPerCategoryOutputDto> GetBooksPerCategoryAsync();
        Task<MostPopularOutputDto> GetMostPopularAsync();
    }
}
