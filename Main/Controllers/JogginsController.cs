using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DataTransferObjects.JoggingDtos;
using Models.IdentityModels;
using Models.JoggingModels;
using Models.WeatherModels;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Main.Controllers
{
    [Route("api/joggings")]
    [ApiController]
    public class JogginsController : ControllerBase
    {
        private readonly IJoggingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IWeatherManager _weatherManager;
        private readonly UserManager<User> _userManager;
        public JogginsController(IJoggingRepository repo, IMapper mapper, 
                                    IWeatherManager weatherManager, UserManager<User> userManager)
        {
            _repo = repo;
            _mapper = mapper;
            _weatherManager = weatherManager;
            _userManager = userManager;
        }

        [HttpGet("", Name = "GetAllJoggings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllJoggings()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            IEnumerable<Jogging> joggings = new List<Jogging>();
            if (role == "Admin")
            {
                joggings = await _repo.GetAllJoggings();
            }
            else if( role == "Jogger")
            {
                joggings = await _repo.GetJoggingsByUsername(userName);
            }
            var joggingDtos = _mapper.Map<IEnumerable<JoggingDto>>(joggings);
            return Ok(joggingDtos);
        }


        [HttpGet("{id}", Name = "GetJoggingById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetJoggingById(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var jogging = await _repo.GetJoggingById(id);
            if (jogging == null)
            {
                return NotFound();
            }
            if (role != "Admin" && jogging.User.UserName != userName)
            {
                return Unauthorized();    
            }
            var joggingDto = _mapper.Map<JoggingDto>(jogging);
            return Ok(joggingDto);
        }

        [HttpPost("", Name = "CreateJogging")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateJogging([FromBody] JoggingCreateDto joggingCreateDto)
        {
            if (joggingCreateDto == null)
            {
                return BadRequest("JoggingCreateDto object is null");
            }
            var searchDate = joggingCreateDto.JoggingDate;
            var jogging = _mapper.Map<Jogging>(joggingCreateDto);
            WeatherServiceResult weatherResult;
            if (searchDate < DateTime.Today)
            {
                weatherResult = await _weatherManager.GetHistoryWeather(joggingCreateDto.Location, searchDate);
            }
            else
            {
                weatherResult = await _weatherManager.GetCurrentOrForecast(joggingCreateDto.Location, searchDate);
            }

            if (weatherResult != null && weatherResult.Succeeded)
            {
                jogging.TemperatureC = weatherResult.TempC;
                jogging.TemperatureF = weatherResult.TempF;
                jogging.WeatherCondition = weatherResult.WeatherCondition;
                jogging.humidity = weatherResult.Humidity;
                jogging.DateCreated = DateTime.Now;
                jogging.DateUpdated = DateTime.Now;

                _repo.CreateJogging(jogging);
                _repo.Save();

                var joggingToReturn = _mapper.Map<JoggingDto>(jogging);
                return CreatedAtRoute("GetJoggingById", new { id = joggingToReturn.Id }, joggingToReturn);
            }
            else
            {
                return StatusCode(422, new { Success = false, ErrorMessage = weatherResult.ErrorMessage });
            }
        }


        [HttpPut("{id}", Name = "UpdateJogging")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateJogging(int id, [FromBody] JoggingUpdateDto joggingUpdateDto)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var jogging = await _repo.GetJoggingById(id);
            if (jogging == null)
            {
                return NotFound();
            }

            if (joggingUpdateDto == null)
            {
                return BadRequest("joggingUpdateDto object is null");
            }
            if (role != "Admin" && jogging.User.UserName != userName)
            {
                return Unauthorized();
            }

            var searchDate = joggingUpdateDto.JoggingDate;
            WeatherServiceResult weatherResult;
            if (searchDate < DateTime.Today)
            {
                weatherResult = await _weatherManager.GetHistoryWeather(joggingUpdateDto.Location, searchDate);
            }
            else
            {
                weatherResult = await _weatherManager.GetCurrentOrForecast(joggingUpdateDto.Location, searchDate);
            }

            if (weatherResult != null && weatherResult.Succeeded)
            {
                jogging.TemperatureC = weatherResult.TempC;
                jogging.TemperatureF = weatherResult.TempF;
                jogging.WeatherCondition = weatherResult.WeatherCondition;
                jogging.humidity = weatherResult.Humidity;
                jogging.DistanceInMeters = joggingUpdateDto.DistanceInMeters;
                jogging.JoggingDurationInMinutes = joggingUpdateDto.JoggingDurationInMinutes;
                jogging.Location = joggingUpdateDto.Location;
                jogging.DateUpdated = DateTime.Now;
                _repo.UpdateJogging(jogging);
                _repo.Save();

                return NoContent();
            }
            else
            {
                return StatusCode(422, new { Success = false, weatherResult.ErrorMessage });
            }
        }


        [HttpDelete("{id}", Name = "DeleteJogging")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteJogging(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var jogging = await _repo.GetJoggingById(id);
            if (jogging == null)
            {
                return NotFound();
            }

            if (role != "Admin" && jogging.User.UserName != userName)
            {
                return Unauthorized();
            }

            _repo.DeleteJogging(jogging);
            _repo.Save();
            return NoContent();
        }


        [HttpGet("{userId}/reports", Name = "GetUserWeeklyReports")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetReports(int userId)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.Users.Where(u => u.Id.Equals(userId)).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            if (user.UserName != userName)
            {
                return Unauthorized();
            }

            var joggings = await _repo.GetJoggingsByUserId(userId);
            var weeks = _repo.GetWeeklyReports(joggings);
            return Ok(weeks);
        }

    }
}
