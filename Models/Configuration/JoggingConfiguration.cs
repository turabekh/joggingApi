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
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now, 
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1001,
                    JoggingDate = new DateTime(2020, 10, 3),
                    DistanceInMeters = 3000,
                    Location = "Philadelphia",
                    JoggingDurationInMinutes = 25,
                    UserId = 2002,
                    TemperatureC = 20,
                    TemperatureF = 60,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1002,
                    JoggingDate = new DateTime(2020, 10, 5),
                    DistanceInMeters = 3500,
                    Location = "Tashkent",
                    JoggingDurationInMinutes = 40,
                    UserId = 2002,
                    TemperatureC = 20,
                    TemperatureF = 60,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1003,
                    JoggingDate = new DateTime(2020, 10, 15),
                    DistanceInMeters = 6000,
                    Location = "London",
                    JoggingDurationInMinutes = 60,
                    UserId = 2002,
                    TemperatureC = 20,
                    TemperatureF = 60,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1004,
                    JoggingDate = new DateTime(2020, 10, 17),
                    DistanceInMeters = 800,
                    Location = "London",
                    JoggingDurationInMinutes = 10,
                    UserId = 2002,
                    TemperatureC = 20,
                    TemperatureF = 60,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1005,
                    JoggingDate = new DateTime(2020, 10, 19),
                    DistanceInMeters = 5300,
                    Location = "London",
                    JoggingDurationInMinutes = 50,
                    UserId = 2002,
                    TemperatureC = 20,
                    TemperatureF = 60,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1006,
                    JoggingDate = new DateTime(2020, 10, 21),
                    DistanceInMeters = 7000,
                    Location = "London",
                    JoggingDurationInMinutes = 80,
                    UserId = 2002,
                    TemperatureC = 20,
                    TemperatureF = 60,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1007,
                    JoggingDate = new DateTime(2020, 10, 23),
                    DistanceInMeters = 500,
                    Location = "London",
                    JoggingDurationInMinutes = 10,
                    UserId = 2002,
                    TemperatureC = 20,
                    TemperatureF = 60,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                },
                new Jogging
                {
                    Id = 1008,
                    JoggingDate = new DateTime(2020, 10, 25),
                    DistanceInMeters = 10000,
                    Location = "London",
                    JoggingDurationInMinutes = 120,
                    UserId = 2002,
                    TemperatureC = 20,
                    TemperatureF = 60,
                    humidity = 30,
                    WeatherCondition = "Clear",
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                }
            );
        }
    }
}
