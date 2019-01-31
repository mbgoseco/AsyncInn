using AsyncInn.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Data
{
    public class AsyncInnDbContext : DbContext
    {
        public AsyncInnDbContext(DbContextOptions<AsyncInnDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add composite key associations here
            modelBuilder.Entity<HotelRoom>().HasKey(hr => new { hr.HotelID, hr.RoomNumberID });
            modelBuilder.Entity<RoomAmenities>().HasKey(ra => new { ra.AmenitiesID, ra.RoomID });

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    ID = 1,
                    Name = "Async Inn",
                    Address = "123 Main St",
                    Phone = "555-867-5309",
                },
                new Hotel
                {
                    ID = 2,
                    Name = "Async Motel",
                    Address = "456 Sunset Blvd",
                    Phone = "555-555-5555"
                },
                new Hotel
                {
                    ID = 3,
                    Name = "Async Resort",
                    Address = "777 Beach Blvd",
                    Phone = "555-111-2222"
                },
                new Hotel
                {
                    ID = 4,
                    Name = "Async Lodge",
                    Address = "501 Mountain Rd",
                    Phone = "555-232-4343"
                },
                new Hotel
                {
                    ID = 5,
                    Name = "Async Cottages",
                    Address = "404 Country Rd",
                    Phone = "555-666-6666"
                }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    ID = 1,
                    Name = "Mini Me",
                    Layout = Layouts.Studio,
                },
                new Room
                {
                    ID = 2,
                    Name = "Lone Wolf",
                    Layout = Layouts.Studio,
                },
                new Room
                {
                    ID = 3,
                    Name = "One Night Stand",
                    Layout = Layouts.OneBedroom,
                },
                new Room
                {
                    ID = 4,
                    Name = "Bachelor Pad",
                    Layout = Layouts.OneBedroom,
                },
                new Room
                {
                    ID = 5,
                    Name = "El Presidente",
                    Layout = Layouts.TwoBedroom,
                },
                new Room
                {
                    ID = 6,
                    Name = "Beef Supreme",
                    Layout = Layouts.TwoBedroom,
                }
            );

            modelBuilder.Entity<Amenities>().HasData(
                new Amenities
                {
                    ID = 1,
                    Name = "Kitchen"
                },
                new Amenities
                {
                    ID = 2,
                    Name = "Coffee Maker"
                },
                new Amenities
                {
                    ID = 3,
                    Name = "Hot Tub"
                },
                new Amenities
                {
                    ID = 4,
                    Name = "Balcony/Patio"
                },
                new Amenities
                {
                    ID = 5,
                    Name = "Mini Bar"
                }
            );
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomAmenities> RoomAmenities { get; set; }
        public DbSet<Amenities> Amenities { get; set; }
    }
}
