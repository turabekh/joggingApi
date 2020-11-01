using AutoMapper;
using Interfaces;
using Main;
using Main.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DataTransferObjects.UserDtos;
using Models.IdentityModels;
using Models.RequestParams;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tests.MockServices;
using Xunit;

namespace Tests.UnitTests
{
    public class UserControllerUnitTest
    {
        UsersController _controller;
        IWeatherManager _weatherManager;
        private Mock<UserManager<User>> _userManager = IdentityServiceMock.MockUserManager<User>(Helper.GetUsers());
        private Mock<RoleManager<Role>> _roleManager = IdentityServiceMock.GetRoleMockManager(Helper.GetRoles());

        public UserControllerUnitTest()
        {
            _weatherManager = new WeatherManagerFake();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));
            _controller = new UsersController(_userManager.Object, _roleManager.Object, mapper);

        }

        [Fact]
        public async Task GetAllUsersWithAdminRole_Returns200OkObjectResultOnSuccess()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act 
            var result = await _controller.GetUsers(new UserParameters { });

            //Assert

            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetAllUsersWithManagerRole_Returns200OkObjectResult()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "manageruser"), new Claim(ClaimTypes.Role, "Manager")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act 
            var result = await _controller.GetUsers(new UserParameters { });

            //Assert

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsPagedResultType()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act 
            var result = await _controller.GetUsers(new UserParameters { }) as OkObjectResult;
            var paginationHeader = _controller.Response.Headers.Where(h => h.Key.Contains("X-Pagination")).FirstOrDefault();
            var paginationValues = paginationHeader.Value.FirstOrDefault();
            //Assert

            Assert.IsType<List<UserDto>>(result.Value);
            Assert.Contains("TotalPages", paginationValues);
            Assert.Contains("PageSize", paginationValues);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkObjectResultOnSuccess()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _controller.ControllerContext.HttpContext.Items.Add("user", new User { Id = 2000, UserName = "adminuser" });

            // Act 
            var result = await _controller.GetUserById(2002);

            //Assert

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetUserById_ReturnsSingleUserDtoTypeOnSuccess()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _controller.ControllerContext.HttpContext.Items.Add("user", new User { Id = 2000, UserName = "adminuser" });

            // Act 
            var result = await _controller.GetUserById(2002) as OkObjectResult;

            //Assert

            Assert.IsType<SingleUserDto>(result.Value);
        }

        [Fact]
        public async Task CreateUser_Returns201StatusResultOnSuccess()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };


            var userCreateDto = new UserCreateDto
            {
                FirstName = "New User",
                LastName = "new User lastName",
                UserName = "newuser",
                Password = "2020NewUser",
                Email = "newuser@gmail.com",
                Roles = new List<string>() { "Manager" }
            };
            // Act 
            var result = await _controller.CreateUser(userCreateDto) as StatusCodeResult;

            //Assert

            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContentObjectResultOnSuccess()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _controller.ControllerContext.HttpContext.Items.Add("user", new User { Id = 2000, UserName = "adminuser" });
            var userUpdateDto = new UserUpdateDto
            {
                FirstName = "admin user firstName",
                LastName = "admin user lastName",
                UserName = "adminuser",
                Email = "adminuser@gmail.com",
                PhoneNumber = "333333333"
            };
            // Act 
            var result = await _controller.UpdateUser(2000, userUpdateDto);

            //Assert

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_Returns403ForbiddenStatusResultWhenUserNotOwner()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _controller.ControllerContext.HttpContext.Items.Add("user", new User { Id = 2000, UserName = "adminuser" });
            var userUpdateDto = new UserUpdateDto
            {
                FirstName = "admin user firstName",
                LastName = "admin user lastName",
                UserName = "adminuser",
                Email = "adminuser@gmail.com",
                PhoneNumber = "333333333"
            };
            // Act 
            var result = await _controller.UpdateUser(2000, userUpdateDto) as StatusCodeResult;

            //Assert

            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task UpdateUserRoles_ReturnsOkObjectResult()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _controller.ControllerContext.HttpContext.Items.Add("user", new User { Id = 2000, UserName = "adminuser" });
            var userRolesDto = new UserRolesDto
            {
                UserId = 2002,
                Roles = new List<string>() { "Jogger" }
            };
            // Act 
            var result = await _controller.UpdateUserRoles(2002, userRolesDto);

            //Assert

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContentResultOnSuccess()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            _controller.ControllerContext.HttpContext.Items.Add("user", new User { Id = 2000, UserName = "adminuser" });

            // Act 
            var result = await _controller.DeleteUser(2002);

            //Assert

            Assert.IsType<NoContentResult>(result);
        }
        
    }
}
