using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DataTransferObjects.JoggingDtos
{
    public class JoggingDto
    {
        public int Id { get; set; }
        public DateTime JoggingDate { get; set; }
        public double DistanceInMeters { get; set; }
        public double DistanceInMiles => (0.000621371 * DistanceInMeters);
        public string Location { get; set; }
        public int JoggingDurationInMinutes { get; set; }
        public int UserId { get; set; }
        public float? TemperatureC { get; set; }
        public float? TemperatureF { get; set; }
        public float? humidity { get; set; }
        public string WeatherCondition { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
