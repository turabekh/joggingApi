using Models.WeatherModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IWeatherManager
    {
        Task<WeatherServiceResult> GetHistoryWeather(string location, DateTime date);
        Task<WeatherServiceResult> GetCurrentOrForecast(string location, DateTime date);
    }
}
