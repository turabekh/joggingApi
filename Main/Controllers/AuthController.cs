﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Interfaces;
using Main.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DataTransferObjects.UserDtos;
using Models.IdentityModels;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Main.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        private readonly ILoggerManager _logger;

        public AuthController(UserManager<User> userManager, IMapper mapper, IAuthManager authManager, ILoggerManager logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authManager = authManager;
            _logger = logger;
        }

        [HttpPost("register", Name = "Register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Register(UserRegisterationDto userRegistrationDto)
        {
            var user = _mapper.Map<User>(userRegistrationDto);

            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);
            if (!result.Succeeded)
            {
                foreach(var err in result.Errors)
                {
                    ModelState.TryAddModelError(err.Code, err.Description);
                }
                _logger.LogError("Invalid model state for the UserRegistration object");
                return UnprocessableEntity(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "Jogger");
            return StatusCode(201, new { Success = true });
        }


        [HttpPost("login", Name = "Login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!await _authManager.ValidateUser(userLoginDto))
            {
                return Unauthorized();
            }
            return Ok(new { Success = true,  Token = await _authManager.CreateToken() });
        }

    }
}
