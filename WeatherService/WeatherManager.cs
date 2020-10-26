using Interfaces;
using Models.WeatherModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService
{
    public class WeatherManager : IWeatherManager
    {
        private IHttpClientFactory _clientFactory;
        private string HisotryBaseUrl = @$"http://api.weatherapi.com/v1/history.json?key=385ee88ceb3a4d96b53204208202610&";
        private string ForecastBaseUrl = @"http://api.weatherapi.com/v1/forecast.json?key=385ee88ceb3a4d96b53204208202610&";
        public WeatherManager(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<WeatherServiceResult> GetHistoryWeather(string location, DateTime date)
        {
            try
            {
                var searchDate = date.ToString("yyyy-MM-dd");
                var client = _clientFactory.CreateClient();
                var weatherData = await client.GetFromJsonAsync<Weather>($"{HisotryBaseUrl}q={location}&dt={searchDate}");
                var forecast = weatherData.forecast.forecastday.FirstOrDefault();
                var result = new WeatherServiceResult
                {
                    Succeeded = true,
                    TempC = forecast.day.avgtemp_c,
                    TempF = forecast.day.avgtemp_f,
                    WeatherCondition = forecast.day.condition.text,
                    Humidity = forecast.day.avghumidity
                };
                return result;
            }
            catch (Exception ex)
            {
                return new WeatherServiceResult { Failed = true, ErrorMessage = ex.Message };
            }
        }

        public async Task<WeatherServiceResult> GetCurrentOrForecast(string location, DateTime date)
        {
            try
            {
                var currentDate = DateTime.Today;
                if (date > currentDate.AddDays(2))
                {
                    return new WeatherServiceResult { Failed = true, ErrorMessage = "Weather Service is only  avaible for the next two days" };
                }
                int dateToTake;
                if (date == currentDate)
                {
                    dateToTake = 0;
                }
                else if (date == currentDate.AddDays(1))
                {
                    dateToTake = 1;
                }
                else
                {
                    dateToTake = 2;
                }
                var client = _clientFactory.CreateClient();
                var weatherData = await client.GetFromJsonAsync<Weather>($"{ForecastBaseUrl}q={location}&days=3");
                var forecast = weatherData.forecast.forecastday.Skip(dateToTake).Take(1).FirstOrDefault();
                var result = new WeatherServiceResult
                {
                    Succeeded = true,
                    TempC = forecast.day.avgtemp_c,
                    TempF = forecast.day.avgtemp_f,
                    WeatherCondition = forecast.day.condition.text,
                    Humidity = forecast.day.avghumidity
                };
                return result;
            }
            catch (Exception ex)
            {
                return new WeatherServiceResult { Failed = true, ErrorMessage = ex.Message };
            }
        }
    }
}
