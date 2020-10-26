using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
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
            );
        }
    }
}
