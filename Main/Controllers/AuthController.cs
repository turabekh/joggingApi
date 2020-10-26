using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public AuthController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpPost("register", Name = "Register")]
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
                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "Jogger");
            return StatusCode(201, new { Success = true });
        }
    }
}
