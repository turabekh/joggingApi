using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherManager _weatherManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherManager weatherManager)
        {
            _logger = logger;
            _weatherManager = weatherManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rng = new Random();
            var data = await _weatherManager.GetCurrentOrForecast("London", DateTime.Today.AddDays(4));
            var data2 = await _weatherManager.GetHistoryWeather("London", new DateTime(2020, 10, 25));
            return Ok("All is good");
        }
    }
}
