using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Configuration;
using Models.IdentityModels;
using Models.JoggingModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class DataContext : IdentityDbContext<User, Role, int>
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new JoggingConfiguration());


            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            modelBuilder.Entity<Jogging>()
                .HasOne(j => j.User)
                .WithMany(u => u.Joggings)
                .HasForeignKey(j => j.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<Jogging>().Property(j => j.UserId).IsRequired();
            modelBuilder.Entity<Jogging>().Property(j => j.JoggingDate).IsRequired();
            modelBuilder.Entity<Jogging>().Property(j => j.DistanceInMeters).IsRequired();
            modelBuilder.Entity<Jogging>().Property(j => j.JoggingDurationInMinutes).IsRequired();
            modelBuilder.Entity<Jogging>().Property(j => j.Location).IsRequired().HasMaxLength(500);
        }


        public DbSet<Jogging> Joggings { get; set; }

    }
}
