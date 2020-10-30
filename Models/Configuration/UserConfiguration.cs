using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Configuration
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private PasswordHasher<User> hasher = new PasswordHasher<User>();
        private int AdminId = 2000;
        private int ManagerId = 2001;
        private int JoggerId = 2002;
        private int JoggerId2 = 2003;
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData
            (
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
            );
        }
    }
}
