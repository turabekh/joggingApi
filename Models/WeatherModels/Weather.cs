using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WeatherModels
{

    public class Weather
    {
        public Current current { get; set; }
        public Forecast forecast { get; set; }
    }

}
