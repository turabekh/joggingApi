using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WeatherModels
{
    public class WeatherServiceResult
    {
        public bool Succeeded { get; set; } = false;
        public bool Failed { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public float TempC { get; set; }
        public float TempF { get; set; }
        public string WeatherCondition { get; set; }
        public float Humidity { get; set; }

    }
}
