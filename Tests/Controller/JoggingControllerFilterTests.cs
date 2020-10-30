using Main;
using Models.DataTransferObjects.JoggingDtos;
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
    public class JoggingControllerFilterTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly HttpClient _client;
        public JoggingControllerFilterTests(TestingWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }



        [Fact]
        public async Task GetAllJoggings_EqualTo40Test()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var joggings = await _client.GetFromJsonAsync<IEnumerable<JoggingDto>>(@"https://localhost:44391/api/joggings?$filter=temperatureC eq 40");
            if (joggings.Count() > 0)
            {
                foreach (var j in joggings)
                {
                    Assert.Equal(40, j.TemperatureC);
                }
            }
        }

        [Fact]
        public async Task GetAllJoggings_LessThan40Test()
        {
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var joggings = await _client.GetFromJsonAsync<IEnumerable<JoggingDto>>(@"https://localhost:44391/api/joggings?$filter=temperatureC lt 40");
            if (joggings.Count() > 0)
            {
                foreach (var j in joggings)
                {
                    Assert.True(j.TemperatureC < 40);
                }
            }
        }

        [Fact]
        public async Task GetAllJoggings_GreaterThan40Test()
        {
            var url = @"https://localhost:44391/api/joggings?$filter=temperatureC gt 40";
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var joggings = await _client.GetFromJsonAsync<IEnumerable<JoggingDto>>(url);
            if (joggings.Count() > 0)
            {
                foreach (var j in joggings)
                {
                    Assert.True(j.TemperatureC > 40);
                }
            }
        }

        [Fact]
        public async Task GetAllJoggings_OR_OperatorTest_LessThan40_Or_GreaterThan60()
        {
            var url = @"https://localhost:44391/api/joggings?$filter=(temperatureC lt 40) Or (temperatureC gt 60)";
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var joggings = await _client.GetFromJsonAsync<IEnumerable<JoggingDto>>(url);
            if (joggings.Count() > 0)
            {
                foreach (var j in joggings)
                {
                    Assert.True((j.TemperatureC < 40) || (j.TemperatureC > 60));
                }
            }
        }

        [Fact]
        public async Task GetAllJoggings_AND_OperatorTest_TemperatureLessThan40_And_JoggingDateGreater1January2019()
        {
            var url = @"https://localhost:44391/api/joggings?$filter=(temperatureC lt 40) And (joggingDate gt 2019-01-01)";
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var joggings = await _client.GetFromJsonAsync<IEnumerable<JoggingDto>>(url);
            if (joggings.Count() > 0)
            {
                foreach (var j in joggings)
                {
                    Assert.True((j.TemperatureC < 40) && (j.JoggingDate > new DateTime(2019, 1, 1)));
                }
            }
        }

        [Fact]
        public async Task GetAllJoggings_AND_Operator_and_OR_operatorTest()
        {
            var url = @"https://localhost:44391/api/joggings?$filter=(joggingDate eq 2020-10-01) And ((distanceInMeters lt 6000) Or (distanceInMeters gt 500))";
            var jwtToken = MockJWTTokens.CreateRoleJWTToken("Admin", "adminuser");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtToken}");
            var joggings = await _client.GetFromJsonAsync<IEnumerable<JoggingDto>>(url);
            if (joggings.Count() > 0)
            {
                foreach (var j in joggings)
                {
                    Assert.True(j.JoggingDate.Date.Equals(new DateTime(2020, 10, 01)) && (j.DistanceInMeters < 6000 || j.DistanceInMeters > 500));
                }
            }
        }
    }
}
