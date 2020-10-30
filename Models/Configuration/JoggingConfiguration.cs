using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.JoggingModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Configuration
{
    public class JoggingConfiguration : IEntityTypeConfiguration<Jogging>
    {
        public void Configure(EntityTypeBuilder<Jogging> builder)
        {
            builder.HasData
            (
                new Jogging
                {
                    Id = 1000,
                    JoggingDate = new DateTime(2020, 10, 1),
                    DistanceInMeters = 2000, 
                    Location = "London",
                    JoggingDurationInMinutes = 20, 
                    UserId = 2002, 
                    TemperatureC = 20, 
                    TemperatureF = 60, 
                    humidity = 30, 
                    WeatherCondition = "Cloudy",
                    DateCreated = DateTime.Now, 
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1001,
                    JoggingDate = new DateTime(2019, 5, 2),
                    DistanceInMeters = 3000,
                    Location = "Philadelphia",
                    JoggingDurationInMinutes = 25,
                    UserId = 2002,
                    TemperatureC = 30,
                    TemperatureF = 90,
                    humidity = 30,
                    WeatherCondition = "Rainy",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1002,
                    JoggingDate = new DateTime(2018, 11, 15),
                    DistanceInMeters = 3500,
                    Location = "Tashkent",
                    JoggingDurationInMinutes = 40,
                    UserId = 2002,
                    TemperatureC = 40,
                    TemperatureF = 100,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1003,
                    JoggingDate = new DateTime(2020, 7, 30),
                    DistanceInMeters = 6000,
                    Location = "Madrid",
                    JoggingDurationInMinutes = 60,
                    UserId = 2002,
                    TemperatureC = 28,
                    TemperatureF = 78,
                    humidity = 30,
                    WeatherCondition = "Snow",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1004,
                    JoggingDate = new DateTime(2020, 8, 2),
                    DistanceInMeters = 800,
                    Location = "Milan",
                    JoggingDurationInMinutes = 10,
                    UserId = 2002,
                    TemperatureC = 15,
                    TemperatureF = 50,
                    humidity = 30,
                    WeatherCondition = "Hot",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1005,
                    JoggingDate = new DateTime(2020, 10, 19),
                    DistanceInMeters = 5300,
                    Location = "Moscow",
                    JoggingDurationInMinutes = 50,
                    UserId = 2002,
                    TemperatureC = 8,
                    TemperatureF = 40,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1006,
                    JoggingDate = new DateTime(2016, 12, 26),
                    DistanceInMeters = 7000,
                    Location = "Warsaw",
                    JoggingDurationInMinutes = 80,
                    UserId = 2002,
                    TemperatureC = -5,
                    TemperatureF = 20,
                    humidity = 30,
                    WeatherCondition = "Warm",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1007,
                    JoggingDate = new DateTime(2019, 12, 31),
                    DistanceInMeters = 500,
                    Location = "New York",
                    JoggingDurationInMinutes = 10,
                    UserId = 2002,
                    TemperatureC = -12,
                    TemperatureF = 9,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1008,
                    JoggingDate = new DateTime(2020, 07, 23),
                    DistanceInMeters = 10000,
                    Location = "London",
                    JoggingDurationInMinutes = 120,
                    UserId = 2002,
                    TemperatureC = 0,
                    TemperatureF = 13,
                    humidity = 30,
                    WeatherCondition = "Tornado",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                }
            );
        }
    }
}
