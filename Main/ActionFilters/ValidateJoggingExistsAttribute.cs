using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.ActionFilters
{
    public class ValidateJoggingExistsAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IJoggingRepository _repo;

        public ValidateJoggingExistsAttribute(ILoggerManager logger, IJoggingRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = (int)context.ActionArguments["id"];
            var jogging = await _repo.GetJoggingById(id);
            if (jogging == null)
            {
                _logger.LogInfo($"Jogging with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("jogging", jogging);
                await next();

            }
        }

        public async Task OnActionExecuted(ActionExecutedContext context) { }
    }
}
