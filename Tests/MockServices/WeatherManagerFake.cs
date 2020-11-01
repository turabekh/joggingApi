using Interfaces;
using Models.WeatherModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests.MockServices
{
    public class WeatherManagerFake : IWeatherManager
    {
        private WeatherServiceResult _weatherResult;
        public WeatherManagerFake()
        {
            _weatherResult = new WeatherServiceResult
            {
                Succeeded = true,
                TempC = 30,
                TempF = 70,
                WeatherCondition = "clear",
                Humidity = 60
            };
        }
        public async Task<WeatherServiceResult> GetCurrentOrForecast(string location, DateTime date)
        {
            return _weatherResult;
        }

        public async Task<WeatherServiceResult> GetHistoryWeather(string location, DateTime date)
        {
            return _weatherResult;
        }
    }
}
