using Interfaces;
using Models.DataTransferObjects.UserDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests.MockServices
{
    public class AuthManagerFake : IAuthManager
    {
        public async Task<string> CreateToken()
        {
            return MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
        }

        public async Task<bool> ValidateUser(UserLoginDto userLoginDto)
        {
            if (userLoginDto.UserName == "notallowed")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
