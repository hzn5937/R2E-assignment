using LibraryManagement.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserCountOutputDto> GetUserCountAsync();
    }
}
