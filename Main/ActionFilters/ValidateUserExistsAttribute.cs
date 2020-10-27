using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.ActionFilters
{
    public class ValidateUserExistsAttribute : IAsyncActionFilter
    {
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;

        public ValidateUserExistsAttribute(UserManager<User> userManager, ILoggerManager logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = (int)context.ActionArguments["id"];
            var user = await _userManager.Users.Where(u => u.Id.Equals(id))
                .Include(u => u.Joggings)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("user", user);
                await next();

            }
        }

        public async Task OnActionExecuted(ActionExecutedContext context) { }
    }
}
