using AutoMapper;
using Interfaces;
using Main;
using Main.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.JoggingDtos;
using Models.IdentityModels;
using Models.RequestParams;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tests.MockServices;
using Xunit;

namespace Tests.UnitTests
{
    public class JoggingControllerUnitTest
    {
        JoggingsController _controller;
        IJoggingRepository _joggingRepo;
        IWeatherManager _weatherManager;
        private Mock<UserManager<User>> _userManager = Helper.MockUserManager<User>(Helper.GetUsers());

        public JoggingControllerUnitTest()
        {
            _joggingRepo = new JoggingRepositoryFake();
            _weatherManager = new WeatherManagerFake();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile())));
            _controller = new JoggingsController(_joggingRepo, mapper, _weatherManager, _userManager.Object);

        }


        [Fact]
        public async Task GetAllJoggingsTest()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act 
            var result = await _controller.GetAllJoggings(new JoggingParameters());

            //Assert

            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task GetAllJoggings_ReturnsListOfJoggingDtos()
        {
            // Arrange
            var totalJoggings = 9;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.GetAllJoggings(new JoggingParameters()) as OkObjectResult;

            // Assert 
            var joggings = Assert.IsType<List<JoggingDto>>(result.Value);
            Assert.Equal(totalJoggings, joggings.Count);
      
        }

        [Fact]
        public async Task GetAllJoggingsWithJoggerRole_ReturnsOnlyJoggerSelfJoggings()
        {
            // Arrange
            var userId = 2002;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.GetAllJoggings(new JoggingParameters()) as OkObjectResult;

            // Assert 
            var joggings = Assert.IsType<List<JoggingDto>>(result.Value);
            foreach(var j in joggings)
            {
                Assert.Equal(userId, j.UserId);
            }
        }


        [Fact]
        public async Task GetJoggingById_ReturnsOkObjectResult()
        {
            // Arrange
            var joggingId = 1000;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "adminuser"), new Claim(ClaimTypes.Role, "Admin")}));
            var jogging = await _joggingRepo.GetJoggingById(joggingId);
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _controller.ControllerContext.HttpContext.Items.Add("jogging", jogging);

            // Act
            var result = await _controller.GetJoggingById(joggingId);

            // Assert 
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetJoggingById_ReturnsStatusCodeResult403WhenNotSelfJogging()
        {
            // Arrange
            var jogggingId = 1000;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            var jogging = await _joggingRepo.GetJoggingById(jogggingId);
            jogging.User = new User { UserName = "someOtherUser" };
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            _controller.ControllerContext.HttpContext.Items.Add("jogging", jogging);

            // Act
            var result = await _controller.GetJoggingById(jogggingId) as StatusCodeResult;

            // Assert 
            Assert.Equal(403, result.StatusCode);
        }


        [Fact]
        public async Task CreateJogging_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            var joggingCreateDto = new JoggingCreateDto
            {
                JoggingDate = DateTime.Now,
                DistanceInMeters = 400,
                Location = "London",
                JoggingDurationInMinutes = 30,
                UserId = 2002
            };

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.CreateJogging(joggingCreateDto);

            // Assert 
            Assert.IsType<CreatedAtRouteResult>(result);
            
        }

        [Fact]
        public async Task CreateJogging_ReturnsBadRequestResultWhenUserDoesNotExist()
        {
            // Arrange
            var nonExistingUserId = int.MaxValue;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            var joggingCreateDto = new JoggingCreateDto
            {
                JoggingDate = DateTime.Now,
                DistanceInMeters = 400,
                Location = "London",
                JoggingDurationInMinutes = 30,
                UserId = nonExistingUserId
            };

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.CreateJogging(joggingCreateDto);

            // Assert 
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateJogging_Returns403UnAuthorizedObjecResultWhenUserNotOwner()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            var joggingCreateDto = new JoggingCreateDto
            {
                JoggingDate = DateTime.Now,
                DistanceInMeters = 400,
                Location = "London",
                JoggingDurationInMinutes = 30,
                UserId = 2001
            };

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.CreateJogging(joggingCreateDto) as StatusCodeResult;

            // Assert 
            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task UpdateJogging_Returns201NoContentObjectResult()
        {
            // Arrange
            var jogggingId = 1000;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            var jogging = await _joggingRepo.GetJoggingById(jogggingId);
            jogging.User = new User { UserName = "joggeruser" };
            var joggingUpdateDto = new JoggingUpdateDto
            {
                JoggingDate = DateTime.Now,
                DistanceInMeters = 5000,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 50
            };


            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            _controller.ControllerContext.HttpContext.Items.Add("jogging", jogging);

            // Act
            var result = await _controller.UpdateJogging(jogggingId, joggingUpdateDto);

            // Assert 
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateJogging_Returns403UnauthorizedResultWhenUserNotOwner()
        {
            // Arrange
            var jogggingId = 1000;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            var jogging = await _joggingRepo.GetJoggingById(jogggingId);
            jogging.User = new User { UserName = "someotheruser" };
            var joggingUpdateDto = new JoggingUpdateDto
            {
                JoggingDate = DateTime.Now,
                DistanceInMeters = 5000,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 50
            };


            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            _controller.ControllerContext.HttpContext.Items.Add("jogging", jogging);

            // Act
            var result = await _controller.UpdateJogging(jogggingId, joggingUpdateDto) as StatusCodeResult;

            // Assert 
            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public async Task DeleteJogging_ReturnsNoContenResultOnSuccess()
        {
            var jogggingId = 1000;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            var jogging = await _joggingRepo.GetJoggingById(jogggingId);
            jogging.User = new User { UserName = "joggeruser" };

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            _controller.ControllerContext.HttpContext.Items.Add("jogging", jogging);

            // Act
            var result = await _controller.DeleteJogging(jogggingId);

            // Assert 
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteJogging_Returns403StatusCodeResultWhenUserNotOwner()
        {
            var jogggingId = 1005;
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { new Claim(ClaimTypes.Name, "joggeruser"), new Claim(ClaimTypes.Role, "Jogger")}));
            var jogging = await _joggingRepo.GetJoggingById(jogggingId);
            jogging.User = new User { UserName = "someotheruser" };

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            _controller.ControllerContext.HttpContext.Items.Add("jogging", jogging);

            // Act
            var result = await _controller.DeleteJogging(jogggingId) as StatusCodeResult;

            // Assert 
            Assert.Equal(403, result.StatusCode);
        }
    }
}
