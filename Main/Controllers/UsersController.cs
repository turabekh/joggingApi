using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Interfaces;
using Main.ActionFilters;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Models;
using Models.DataTransferObjects.UserDtos;
using Models.IdentityModels;
using Models.RequestParams;
using Newtonsoft.Json;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Main.Controllers
{

    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager,
                                IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }


        [HttpGet("", Name = "GetUsers"), Authorize(Roles = "Admin, Manager")]
        [EnableQuery]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParamters)
        {
            var users = _userManager.Users
                .Include(u => u.Joggings)
                .ToList();
            var pagedUsers = PagedList<User>.ToPagedList(users, userParamters.PageNumber, userParamters.PageSize);
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(pagedUsers);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagedUsers.MetaData));
            return Ok(userDtos);
        }


        [HttpGet("{id}", Name = "GetUserById"), Authorize(Roles = "Admin, Manager, Jogger")]
        [EnableQuery]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUserById(int id)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var user = HttpContext.Items["user"] as User;
            if (role == "Jogger" && user.UserName != userName)
            {
                return StatusCode(403);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<SingleUserDto>(user);
            userDto.Roles = roles;

            return Ok(userDto);
        }


        [HttpPost("", Name = "CreateUser"), Authorize(Roles = "Admin, Manager")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            var user = _mapper.Map<User>(userCreateDto);
            try
            {
                var result = await _userManager.CreateAsync(user, userCreateDto.Password);
                if (!result.Succeeded)
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.TryAddModelError(err.Code, err.Description);
                    }
                    return UnprocessableEntity(ModelState);
                }
                foreach(var role in userCreateDto.Roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        ModelState.TryAddModelError("", $"Role: {role} does not exist");
                        return UnprocessableEntity(ModelState);
                    }
                }
                await _userManager.AddToRolesAsync(user, userCreateDto.Roles);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, ErrorMessage = ex.Message });
            }
        }


        [HttpPut("{id}", Name = "UpdateUser"), Authorize(Roles = "Admin, Manager, Jogger")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            var role = claimsIdentity.FindFirst(ClaimTypes.Role)?.Value;
            var user = HttpContext.Items["user"] as User;
            if (role == "Jogger" && user.UserName != userName)
            {
                return StatusCode(403);
            }
            user.FirstName = userUpdateDto.FirstName;
            user.LastName = userUpdateDto.LastName;
            user.UserName = userUpdateDto.UserName;
            user.PhoneNumber = userUpdateDto.PhoneNumber;
            user.Email = userUpdateDto.Email;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }

        [HttpPost("{id}/roles", Name = "UpdateUserRoles"), Authorize(Roles = "Admin, Manager")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateUserRoles(int id, [FromBody] UserRolesDto userRolesDto)
        {
            var user = HttpContext.Items["user"] as User;
            foreach (var role in userRolesDto.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    ModelState.TryAddModelError("", $"Role: {role} does not exist");
                    return BadRequest(ModelState);
                }
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, userRolesDto.Roles);
            return Ok(new { Success = true });

        }

        [HttpDelete("{id}", Name = "DeleteUser"), Authorize(Roles = "Admin, Manager")]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = HttpContext.Items["user"] as User;
            await _userManager.DeleteAsync(user);
            return NoContent();
        }



    }
}
