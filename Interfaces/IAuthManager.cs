using Models.DataTransferObjects.UserDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(UserLoginDto userLoginDto);
        Task<string> CreateToken();

    }
}
