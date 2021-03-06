﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using Interfaces;
using Main.ActionFilters;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DataTransferObjects.JoggingDtos;
using Models.IdentityModels;
using Models.JoggingModels;
using Models.RequestParams;
using Models.WeatherModels;
using Newtonsoft.Json;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Main.Controllers
{
    [Route("api/joggings")]
    [ApiController]
    public class JoggingsController : ControllerBase
    {
        private readonly IJoggingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IWeatherManager _weatherManager;
        private readonly UserManager<User> _userManager;

        public JoggingsController(IJoggingRepository repo, IMapper mapper, IWeatherManager weatherManager, 
                            UserManager<User> userManager)
        {
            _repo = repo;
            _mapper = mapper;
            _weatherManager = weatherManager;
            _userManager = userManager;
        }

        [HttpGet("", Name = "GetAllJoggings"), Authorize(Roles = "Admin, Jogger")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllJoggings([FromQuery] JoggingParameters joggingParameters)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            PagedList<Jogging> joggings = role == "Admin" ? await _repo.GetAllJoggings(joggingParameters) : 
                await _repo.GetJoggingsByUsername(userName, joggingParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(joggings.MetaData));
            var joggingDtos = _mapper.Map<IEnumerable<JoggingDto>>(joggings);
            return Ok(joggingDtos);
        }


        [HttpGet("{id}", Name = "GetJoggingById"), Authorize(Roles = "Admin, Jogger")]
        [EnableQuery]
        [ServiceFilter(typeof(ValidateJoggingExistsAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetJoggingById(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var jogging = HttpContext.Items["jogging"] as Jogging;
            if (role != "Admin" && jogging.User.UserName != userName)
            {
                return StatusCode(403);    
            }
            var joggingDto = _mapper.Map<JoggingDto>(jogging);
            return Ok(joggingDto);
        }

        [HttpPost("", Name = "CreateJogging"), Authorize(Roles = "Admin, Jogger")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateJogging([FromBody] JoggingCreateDto joggingCreateDto)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var user = _userManager.Users.Where(u => u.Id.Equals(joggingCreateDto.UserId)).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("", $"User with userid: {joggingCreateDto.UserId} does not exist");
                return BadRequest(ModelState);
            }
            if (user.UserName != userName && role != "Admin" )
            {
                return StatusCode(403);
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
                return StatusCode(422, new { Success = false, weatherResult.ErrorMessage });
            }
        }


        [HttpPut("{id}", Name = "UpdateJogging"), Authorize(Roles = "Admin, Jogger")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateJoggingExistsAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateJogging(int id, [FromBody] JoggingUpdateDto joggingUpdateDto)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var jogging = HttpContext.Items["jogging"] as Jogging;

            if (joggingUpdateDto == null)
            {
                return BadRequest("joggingUpdateDto object is null");
            }
            if (role != "Admin" && jogging.User.UserName != userName)
            {
                return StatusCode(403);
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


        [HttpDelete("{id}", Name = "DeleteJogging"), Authorize(Roles = "Admin, Jogger")]
        [ServiceFilter(typeof(ValidateJoggingExistsAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteJogging(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var jogging = HttpContext.Items["jogging"] as Jogging;

            if (role != "Admin" && jogging.User.UserName != userName)
            {
                return StatusCode(403);
            }

            _repo.DeleteJogging(jogging);
            _repo.Save();
            return NoContent();
        }


        [HttpGet("{id}/reports", Name = "GetUserWeeklyReports"), Authorize(Roles = "Admin, Jogger")]
        [EnableQuery]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetReports(int id, [FromQuery] ReportParameters reportParameters)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var user = HttpContext.Items["user"] as User;

            if (user.UserName != userName && role != "Admin")
            {
                return StatusCode(403);
            }

            var joggings = await _repo.GetJoggingsByUserId(id);
            var weeks = _repo.GetWeeklyReports(joggings, reportParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(weeks.MetaData));
            return Ok(weeks);
        }

    }
}
