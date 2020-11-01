using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.IdentityModels;
using Models.JoggingModels;
using Models.RequestParams;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Tests
{
    public static class Helper
    {

        public static List<Jogging> GetJoggings()
        {
            var joggings = new List<Jogging>()
            {
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
                    DateUpdated = DateTime.Now,
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
            };
            return joggings;
        }


        public static List<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role
                {
                    Id = 1,
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new Role
                {
                    Id = 2,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new Role
                {
                    Id = 3,
                    Name = "Jogger",
                    NormalizedName = "JOGGER"
                }
            };
            return roles;
        }

        public static List<User> GetUsers()
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            int AdminId = 2000;
            int ManagerId = 2001;
            int JoggerId = 2002;
            int JoggerId2 = 2003;
            var users = new List<User>()
            {
                new User
                {
                    Id = AdminId,
                    UserName = "adminuser",
                    NormalizedUserName = "ADMINUSER",
                    Email = "adminuser@gmail.com",
                    NormalizedEmail = "ADMINUSER@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "2020AdminUser"),
                    SecurityStamp = string.Empty
                },
                new User
                {
                    Id = ManagerId,
                    UserName = "manageruser",
                    NormalizedUserName = "MANAGERUSER",
                    Email = "manageruser@gmail.com",
                    NormalizedEmail = "MANAGERUSER@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "2020ManagerUser"),
                    SecurityStamp = string.Empty
                },
                new User
                {
                    Id = JoggerId,
                    UserName = "joggeruser",
                    NormalizedUserName = "JOGGERUSER",
                    Email = "joggeruser@gmail.com",
                    NormalizedEmail = "JOGGERUSER@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "2020JoggerUser"),
                    SecurityStamp = string.Empty
                },
                new User
                {
                    Id = JoggerId2,
                    UserName = "userWithoutJoggings",
                    NormalizedUserName = "USERWITHOUTJOGGINS",
                    Email = "userWithoutJoggings@gmail.com",
                    NormalizedEmail = "USERWITHOUTJOGGINS@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "2020JoggerUser"),
                    SecurityStamp = string.Empty
                }
            };
            return users;
        }
    }
}
