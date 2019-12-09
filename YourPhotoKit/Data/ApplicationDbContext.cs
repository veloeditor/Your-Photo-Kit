using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourPhotoKit.Models;

namespace YourPhotoKit.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<GearItem> GearItems { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<GearType> GearType { get; set; }
        public DbSet<TripPhoto> TripPhotos { get; set; }
        public DbSet<TripGear> TripGear { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Restrict deletion of a gear item that has been assigned to a trip
            modelBuilder.Entity<GearItem>()
                .HasMany(g => g.TripGear)
                .WithOne(l => l.GearItem)
                .OnDelete(DeleteBehavior.Restrict);

            ApplicationUser user = new ApplicationUser
            {
                FirstName = "Brian",
                LastName = "Wilson",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = "7f434309-a4d9-48e9-9ebb-8803db794577",
                Id = "00000000-ffff-ffff-ffff-ffffffffffff"
            };

            var passwordHash = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHash.HashPassword(user, "Admin8*");
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            modelBuilder.Entity<GearType>().HasData(
                new GearType()
                {
                    GearTypeId = 1,
                    Label = "Camera Body"
                },
                new GearType()
                {
                    GearTypeId = 2,
                    Label = "Lens"
                },
                new GearType()
                {
                    GearTypeId = 3,
                    Label = "Lens Adapter"
                },
                new GearType()
                 {
                     GearTypeId = 4,
                     Label = "Filter"
                 },
                new GearType()
                  {
                      GearTypeId = 5,
                      Label = "Tripod"
                  },
                new GearType()
                   {
                       GearTypeId = 6,
                       Label = "Rain Gear"
                   },
                new GearType()
                    {
                        GearTypeId = 7,
                        Label = "Cold Gear"
                    },
                new GearType()
                {
                    GearTypeId = 8,
                    Label = "Bag/Backpack"
                },
                new GearType()
                {
                    GearTypeId = 9,
                    Label = "Cleaning"
                },
                new GearType()
                {
                    GearTypeId = 10,
                    Label = "Misc"
                }

                );

        }
    }
}
