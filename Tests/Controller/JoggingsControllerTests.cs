using Main;
using Microsoft.Data.SqlClient.Server;
using Models.DataTransferObjects.JoggingDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Controller
{
    public class JoggingsControllerTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        public JoggingsControllerTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task GetJoggings_WhenCalled_Returns401UnAuthorizedResponse()
        {
            var response = await _client.GetAsync("/api/joggings");
            Assert.Equal(401, (int)response.StatusCode);   
        }

        [Fact]
        public async Task GetSingleJoggin_WhenCalled_Returns401UnAuthorizedResponse()
        {
            var response = await _client.GetAsync("/api/joggings/1000");
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateJogging_WhenCalled_Returns401UnAuthorizedResponse()
        {
            var response = await _client.PostAsync("/api/joggings", null);
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateJogging_WhenCalled_Returns401UnAuthorizedResponse()
        {
            var response = await _client.PutAsync("/api/joggings/1000", null);
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteJogging_WhenCalled_Returns401UnAuthorizedResponse()
        {
            var response = await _client.DeleteAsync("/api/joggings/1000");
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetJoggingReports_WhenCalled_Returns401UnAuthorizedResponse()
        {
            var response = await _client.GetAsync("/api/joggings/1/reports");
            Assert.Equal(401, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetJoggingsWithJoggerRole_WhenCalledReturns200Ok()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings");
            response.EnsureSuccessStatusCode();
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetJoggingsWithJoggerRole_WhenCalledReturnsJoggingsOnlyJoggerUserCreated()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var joggerUserId2002Joggings = await _client.GetFromJsonAsync<IEnumerable<JoggingDto>>("/api/joggings");
            foreach(var j in joggerUserId2002Joggings)
            {
                Assert.Equal(2002, j.UserId);
            }
        }

        [Fact]
        public async Task GetJoggingsWithManagerRole_WhenCalledReturns403Forbidden()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Manager", "manageruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings");
            Assert.Equal(403, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetJoggingsWithAdminRole_WhenCalledReturnsAllJoggingsWithPagingInfoInResponseHeader()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings");
            var xPaginationResponseHeader = response.Headers.Where(h => h.Key == "X-Pagination").FirstOrDefault();
            var paginationValues = xPaginationResponseHeader.Value.FirstOrDefault();
            Assert.Contains("TotalPages", paginationValues);
            Assert.Contains("PageSize", paginationValues);

        }

        [Fact]
        public async Task GetJoggingsWithJoggerRoleWhoHasNotAnyJoggings_WhenCalledReturnsEmptyList()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "userWithoutJoggings");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var joggerUserId2003Joggings = await _client.GetFromJsonAsync<IEnumerable<JoggingDto>>("/api/joggings");
            Assert.Empty(joggerUserId2003Joggings);

        }


        [Fact]
        public async Task GetSingleJoggingById_WhenCalledReturns404()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings/565415");
            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetSingleJogginWithManagerRole_WhenCalledReturns403Forbidded()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Manager", "manageruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings/1000");
            Assert.Equal(403, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetSingleJoggingWithAdminRole_WhenCalledReturns200Ok()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings/1000");
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetSingleJoggingWithAdminRole_WhenCalledReturnsSingleJogging()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var jogging = await _client.GetFromJsonAsync<JoggingDto>("/api/joggings/1000");
            Assert.IsType<JoggingDto>(jogging);
        }

        [Fact]
        public async Task GetSingleJoggingWithJoggerRole_ReturnsJoggingOnlyJoggerUserCreated()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var jogging = await _client.GetFromJsonAsync<JoggingDto>("/api/joggings/1000");
            Assert.Equal(2002, jogging.UserId);
        }

        [Fact]
        public async Task GetSingleJoggingWithJoggerRole_Returns403ForbiddedForJoggingNotCreatedByThisJogger()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "userWithoutJoggings");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings/1000");
            Assert.Equal(403, (int)response.StatusCode);
        }


        [Fact]
        public async Task CreateJoggingWithAdminRole_Returns201CreatedAtRoute()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 2500,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
                UserId = 2002
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            Assert.Equal(201, (int)response.StatusCode);
        }


        [Fact]
        public async Task CreateJoggingWithAdminRole_Returns201CreatedResponseHeaderIncludesLocationKey()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 2500,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
                UserId = 2002
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            var responseHeader = response.Headers.Where(r => r.Key == "Location").FirstOrDefault();
            Assert.Contains("Location", responseHeader.Key);
        }

        [Fact]
        public async Task CreateJoggingWithJoggerRole_Returns201CreatedAtRouteResponse()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 2500,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
                UserId = 2002
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            Assert.Equal(201, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateJoggingWithJoggerRole_Returns403ForbiddenIfNotSameJogger()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 2500,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
                UserId = 2001
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            Assert.Equal(403, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateJogging_Returns422BadRequestValidatesUserIdField()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 2500,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            Assert.NotEqual(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateJogging_Returns422BadRequestValidatesJoggingDate()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                DistanceInMeters = 2500,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
                UserId = 2002
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact] 
        public async Task CreateJogging_Returns422BadRequestValidatesLocationField()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 2500,
                JoggingDurationInMinutes = 30,
                UserId = 2002
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateJogging_Returns422BadRequestValidatesJoggingDurationInMinutesField()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 2500,
                Location = "Philadelphia",
                UserId = 2002
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task CreateJogging_Returns422BadRequestValidatesDistanceInMetersField()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
                UserId = 2002
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }


        [Fact]
        public async Task UpdateJogging_Returns404NotFound()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingUpdateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 5000,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
            };
            var response = await _client.PutAsJsonAsync("/api/joggings/654651", requestBody);
            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateJogging_Returns403ForbiddenWhenUserIsNotOwnerOfJogging()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "userWithoutJoggings");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingUpdateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 5000,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
            };
            var response = await _client.PutAsJsonAsync("/api/joggings/1000", requestBody);
            Assert.Equal(403, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateJogging_ReturnsBadRequestWithoutRequestBody()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingUpdateDto();
            var response = await _client.PutAsJsonAsync("/api/joggings/1000", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task UpdateJogging_ReturnsBadRequestWithoutInvalidRequestBody()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingUpdateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                Location = "Philadelphia",
            };
            var response = await _client.PutAsJsonAsync("/api/joggings/1000", requestBody);
            Assert.Equal(422, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteJoggingWithAdminUser_Returns404NotFoundWithInvalidJoggingId()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.DeleteAsync("/api/joggings/651115");
            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteJogging_Returns403ForbiddenWhenUserIsNotOwnerOfJogging()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "userWithoutJoggings");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.DeleteAsync("/api/joggings/1000");
            Assert.Equal(403, (int)response.StatusCode);
        }

        [Fact]
        public async Task DeleteJoggingAdminRole_Returns204NoContentOnSuccess()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 2500,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
                UserId = 2002
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            var newlyCreatedJogging = await _client.GetFromJsonAsync<JoggingDto>(response.Headers.Location);
            var deleteResponse = await _client.DeleteAsync($"/api/joggings/{newlyCreatedJogging.Id}");
            Assert.Equal(204, (int)deleteResponse.StatusCode);
        }

        [Fact]
        public async Task DeleteJoggingWithJoggerRole_Returns204NoContentOnSucessWhenUserIsOwnerOfJogging()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var requestBody = new JoggingCreateDto
            {
                JoggingDate = new DateTime(2020, 10, 29),
                DistanceInMeters = 2500,
                Location = "Philadelphia",
                JoggingDurationInMinutes = 30,
                UserId = 2002
            };
            var response = await _client.PostAsJsonAsync("/api/joggings", requestBody);
            var newlyCreatedJogging = await _client.GetFromJsonAsync<JoggingDto>(response.Headers.Location);
            var deleteResponse = await _client.DeleteAsync($"/api/joggings/{newlyCreatedJogging.Id}");
            Assert.Equal(204, (int)deleteResponse.StatusCode);
        }

        [Fact]
        public async Task GetWeeklyReportsWithAdminRole_Returns200Ok()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings/2002/reports");
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetWeeklyReportsWithJoggerRole_Returns200OkWhenJoggerIsOwner()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "joggeruser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings/2002/reports");
            Assert.Equal(200, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetWeeklyReportsWithJoggerRole_Returns403ForbiddenWhenJoggerIsNotOwner()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Jogger", "userWithoutJoggings");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings/2002/reports");
            Assert.Equal(403, (int)response.StatusCode);
        }

        [Fact]
        public async Task GetWeeklyReports_ReturnsPaginationInfoInResponseHeader()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var response = await _client.GetAsync("/api/joggings/2002/reports");
            var xPaginationResponseHeader = response.Headers.Where(h => h.Key == "X-Pagination").FirstOrDefault();
            var paginationValues = xPaginationResponseHeader.Value.FirstOrDefault();
            Assert.Contains("TotalPages", paginationValues);
            Assert.Contains("PageSize", paginationValues);
        }
    }
}
