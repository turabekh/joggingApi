using AutoMapper;
using Interfaces;
using LoggerService;
using Main;
using Main.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.UserDtos;
using Models.IdentityModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tests.MockServices;
using Xunit;

namespace Tests.UnitTests
{
    public class AuthControllerUnitTest
    {
        AuthController _controller;
        private IAuthManager _authManager;
        private Mock<UserManager<User>> _userManager = IdentityServiceMock.MockUserManager<User>(Helper.GetUsers());
        private ILoggerManager _logger;
        
        public AuthControllerUnitTest()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));
            _logger = new LoggerManager();
            _authManager = new AuthManagerFake();
            _controller = new AuthController(_userManager.Object, mapper, _authManager, _logger);

        }

        [Fact]
        public async Task Register_Returns201ObjectResultOnSuccess()
        {
            var userRegisterDto = new UserRegisterationDto
            {
                FirstName = "New User",
                LastName = "new User lastName",
                UserName = "newuser",
                Password = "2020NewUser",
                Email = "newuser@gmail.com",
            };
            // Act 
            var result = await _controller.Register(userRegisterDto);
            var statusResult = result as ObjectResult;

            //Assert
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, statusResult.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsOkObjectResultOnSuccess()
        {
            var userLoginDto = new UserLoginDto
            {
                UserName = "joggeruser",
                Password = "2020JoggerUser",

            };
            // Act 
            var result = await _controller.Login(userLoginDto);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsJWTTokenWithUsernameAndRolesClaimsOnSuccess()
        {
            var userLoginDto = new UserLoginDto
            {
                UserName = "joggeruser",
                Password = "2020JoggerUser",

            };
            // Act 
            var result = await _controller.Login(userLoginDto) as OkObjectResult;
            var jwtResponse = result.Value.ToString();

            //Assert
            Assert.Contains("Token", jwtResponse);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorizedResultOnInvalidToken()
        {
            var userLoginDto = new UserLoginDto
            {
                UserName = "notallowed",
                Password = "2020NotAllowed",

            };
            // Act 
            var result = await _controller.Login(userLoginDto);

            //Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

    }
}
