using Main;
using Models.DataTransferObjects.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Controller
{
    public class AuthControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        public AuthControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task RegisterUser_Returns201CreatedOnSuccess()
        {
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var email = $"{Guid.NewGuid()}@gmail.com";
            var requestBody = new UserRegisterationDto
            {
                FirstName = "someUser",
                LastName = "someUserLastName",
                UserName = userName,
                Password = password,
                Email = email
            };
            var response = await _client.PostAsJsonAsync("/api/auth/register", requestBody);
            Assert.Equal(201, (int)response.StatusCode);
        }

        [Fact]
        public async Task RegisterUserWithInvalidData_Returns422BadRequest()
        {
            var requestBody = new UserRegisterationDto
            {
                FirstName = "someUser",
                LastName = "someUserLastName",
            };
            var response = await _client.PostAsJsonAsync("/api/auth/register", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task Login_Returns200okAndJWTToken()
        {
            var requestBody = new UserLoginDto
            {
                UserName = "joggeruser",
                Password = "2020JoggerUser"
            };
            var response = await _client.PostAsJsonAsync("/api/auth/login", requestBody);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(200, (int)response.StatusCode);
            Assert.Contains("token", responseBody);
        }

        [Fact]
        public async Task LoginUser_Returns401UnauthorizedWithInvalidData()
        {
            var requestBody = new UserLoginDto
            {
                UserName = "someuser",
                Password = Guid.NewGuid().ToString()
            };
            var response = await _client.PostAsJsonAsync("/api/auth/login", requestBody);
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task LoginUser_ReturnBadRequestWithInvalidData()
        {
            var requestBody = new UserLoginDto
            {
                UserName = "someuser"
            };
            var response = await _client.PostAsJsonAsync("/api/auth/login", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }

    }
}
