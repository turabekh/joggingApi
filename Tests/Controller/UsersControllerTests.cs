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
    public class UsersControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        public UsersControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task GetAllUsersWithAdminRole_Returns200OkAndListUserDtos()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/users");
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetAllUsersWithManagerRole_Returns200OkAndListUserDtos()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Manager", "manager");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/users");
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetAllUsersWithAdminRole_ReturnsListOfUsers()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var users = await _client.GetFromJsonAsync<IEnumerable<UserDto>>("/api/users");
            Assert.IsType<List<UserDto>>(users);
        }

        [Fact]
        public async Task GetAllUsersWithManagerRole_ReturnsListOfUsers()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Manager", "manageruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var users = await _client.GetFromJsonAsync<IEnumerable<UserDto>>("/api/users");
            Assert.IsType<List<UserDto>>(users);
        }

        [Fact]
        public async Task GetAllUsersWithJoggerRole_Returns403Forbidden()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/users");
            Assert.Equal(403, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsPagingInfoInResponseHeader()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/users");
            var xPaginationResponseHeader = response.Headers.Where(h => h.Key == "X-Pagination").FirstOrDefault();
            var paginationValues = xPaginationResponseHeader.Value.FirstOrDefault();
            Assert.Contains("TotalPages", paginationValues);
            Assert.Contains("PageSize", paginationValues);
        }

        [Fact]
        public async Task GetSingleUserWithAdminRole_ReturnsSingleUser()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var user = await _client.GetFromJsonAsync<UserDto>("/api/users/2002");
            if (user != null)
            {
                Assert.IsType<UserDto>(user);
            }
        }

        [Fact]
        public async Task GetSingleUser_Returns404NotFound()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync($"/api/users/{int.MaxValue}");
            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateUserWithAdminRole_Return201CreatedResponse()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var email = $"{Guid.NewGuid()}@gmail.com";
            var requestBody = new UserCreateDto
            {
                FirstName = "someUser",
                LastName = "someUserLastName",
                UserName = userName,
                Password = password, 
                Email = email,
                Roles = new List<string>() { "Jogger"}
            };
            var response = await _client.PostAsJsonAsync("/api/users", requestBody);
            Assert.Equal(201, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateUserWithManagerRole_Returns201CreatedResponse()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Manager", "manageruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var email = $"{Guid.NewGuid()}@gmail.com";
            var requestBody = new UserCreateDto
            {
                FirstName = "someUser",
                LastName = "someUserLastName",
                UserName = userName,
                Password = password,
                Email = email,
                Roles = new List<string>() { "Jogger" }
            };
            var response = await _client.PostAsJsonAsync("/api/users", requestBody);
            Assert.Equal(201, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateUserWithJoggerRole_Returns403Forbidded()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var email = $"{Guid.NewGuid()}@gmail.com";
            var requestBody = new UserCreateDto
            {
                FirstName = "someUser",
                LastName = "someUserLastName",
                UserName = userName,
                Password = password,
                Email = email,
                Roles = new List<string>() { "Jogger" }
            };
            var response = await _client.PostAsJsonAsync("/api/users", requestBody);
            Assert.Equal(403, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateUserWithInvalidDataWithoutUserName_Returns422BadRequest()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var email = $"{Guid.NewGuid()}@gmail.com";
            var requestBody = new UserCreateDto
            {
                FirstName = "someUser",
                LastName = "someUserLastName",
                Email = email,
                Roles = new List<string>() { "Jogger" }
            };
            var response = await _client.PostAsJsonAsync("/api/users", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserWithAdminRole_Returns204NoContentOnSuccess()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new UserUpdateDto
            {
                FirstName = "UpdatedUser",
                LastName = "UpdatedLasName",
                PhoneNumber = "55525222",
                Email = "UpdateEmail@gmail.com",
                UserName = "joggeruser"
            };
            var response = await _client.PutAsJsonAsync("/api/users/2002", requestBody);
            Assert.Equal(204, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserWithManagerRole_Returns204NoContentOnSuccess()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Manager", "manageruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new UserUpdateDto
            {
                FirstName = "UpdatedUser",
                LastName = "UpdatedLasName",
                PhoneNumber = "55525222",
                Email = "UpdateEmail@gmail.com",
                UserName = "joggeruser"
            };
            var response = await _client.PutAsJsonAsync("/api/users/2002", requestBody);
            Assert.Equal(204, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserWithJoggerRole_Returns403Forbidden()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new UserUpdateDto
            {
                FirstName = "UpdatedUser",
                LastName = "UpdatedLasName",
                PhoneNumber = "55525222",
                Email = "UpdateEmail@gmail.com",
                UserName = "joggeruser"
            };
            var response = await _client.PutAsJsonAsync("/api/users/2002", requestBody);
            Assert.Equal(403, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserWithInvalidData_Returns422BadRequest()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new UserUpdateDto
            {
                FirstName = "UpdatedUser",
                LastName = "UpdatedLasName",
                PhoneNumber = "55525222",
                Email = "UpdateEmail@gmail.com",
            };
            var response = await _client.PutAsJsonAsync("/api/users/2002", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteUserWithAdminRole_Returns204NoContentOnSuccess()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var email = $"{Guid.NewGuid()}@gmail.com";
            var requestBody = new UserCreateDto
            {
                FirstName = "someUser",
                LastName = "someUserLastName",
                UserName = userName,
                Password = password,
                Email = email,
                Roles = new List<string>() { "Jogger" }
            };
            var users = await _client.GetFromJsonAsync<IEnumerable<UserDto>>("/api/users");
            if (users.Count() > 4)
            {
                var newlyCreatedUser = users.LastOrDefault();
                var deleteResponse = await _client.DeleteAsync($"/api/users/{newlyCreatedUser.Id}");
                Assert.Equal(204, (int)deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task DeleteUserWithManagerRole_Returns204NoContentOnSuccess()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Manager", "manageruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var email = $"{Guid.NewGuid()}@gmail.com";
            var requestBody = new UserCreateDto
            {
                FirstName = "someUser",
                LastName = "someUserLastName",
                UserName = userName,
                Password = password,
                Email = email,
                Roles = new List<string>() { "Jogger" }
            };
            var users = await _client.GetFromJsonAsync<IEnumerable<UserDto>>("/api/users");
            if (users.Count() > 4)
            {
                var newlyCreatedUser = users.LastOrDefault();
                var deleteResponse = await _client.DeleteAsync($"/api/users/{newlyCreatedUser.Id}");
                Assert.Equal(204, (int)deleteResponse.StatusCode);
            }
        }

        [Fact]
        public async Task DeleteUserWithJoggerRole_Returns403Forbidden()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var deleteResponse = await _client.DeleteAsync($"/api/users/{int.MaxValue}");
            Assert.Equal(403, (int)deleteResponse.StatusCode);
        }



    }
}
