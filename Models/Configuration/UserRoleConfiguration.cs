using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasData
            (
                new UserRole
                {
                    UserId = 2000,
                    RoleId = 2
                },
                new UserRole
                {
                    UserId = 2001,
                    RoleId = 1
                },
                new UserRole
                {
                    UserId = 2002,
                    RoleId = 3
                }
            );
        }
    }
}
