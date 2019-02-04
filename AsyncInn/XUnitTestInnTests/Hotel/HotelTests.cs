using System;
using Xunit;
using AsyncInn.Models;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models.Services;
using System.Collections.Generic;

namespace XUnitTestInnTests
{
    public class HotelTests
    {
        /// <summary>
        /// Getter/Setter Tests
        /// </summary>
        [Fact]
        public void CanGetNameOfHotel()
        {
            Hotel hotel = new Hotel();
            hotel.Name = "Async Motel";

            Assert.Equal("Async Motel", hotel.Name);
        }

        [Fact]
        public void CanSetNameOfHotel()
        {
            Hotel hotel = new Hotel();
            hotel.Name = "Async Motel";
            hotel.Name = "Async Inn";

            Assert.Equal("Async Inn", hotel.Name);
        }

        [Fact]
        public void CannotSetPhoneNumberAsLong()
        {
            Hotel hotel = new Hotel();
            hotel.Phone = "123-456-7890";

            long newNumber = 5558675309;

            Assert.IsNotType<string>(newNumber);
        }

        /// <summary>
        /// Create Tests
        /// </summary>
        [Fact]
        public async void CanCreateHotel()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreateHotel").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel hotel = new Hotel();
                hotel.ID = 1;
                hotel.Name = "Async Hotel";
                hotel.Address = "123 Main St";
                hotel.Phone = "555-555-5555";

                HotelManagementService hotelService = new HotelManagementService(context);
                await hotelService.CreateHotel(hotel);

                Hotel result = await context.Hotels.FirstOrDefaultAsync(h => h.ID == hotel.ID);

                Assert.Equal(hotel, result);
            }
        }

        [Fact]
        public async void CreatedIncompleteHotel()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreateIncompleteHotel").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel hotel = new Hotel();
                hotel.ID = 1;
                hotel.Name = "Async Inncomplete";

                HotelManagementService hotelService = new HotelManagementService(context);
                await hotelService.CreateHotel(hotel);

                Hotel result = await context.Hotels.FirstOrDefaultAsync(h => h.Phone != null);

                Assert.NotEqual(hotel, result);
            }
        }

        /// <summary>
        /// Read Tests
        /// </summary>
        [Fact]
        public async void CanReadIndividualHotel()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("ReadSingleHotel").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel resort = new Hotel();
                resort.ID = 1;
                resort.Name = "Async Resort";
                resort.Address = "456 Beach Rd";
                resort.Phone = "555-555-5555";

                HotelManagementService hotelService = new HotelManagementService(context);
                await hotelService.CreateHotel(resort);

                Hotel result = await hotelService.GetHotel(1);

                Assert.Equal(resort, result);
            }
        }

        [Fact]
        public async void CannotReadMissingHotelID()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CantReadMissingHotel").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel hotel = new Hotel();
                hotel.ID = 1;
                hotel.Name = "Async Resort";
                hotel.Address = "456 Beach Rd";
                hotel.Phone = "555-555-5555";

                Hotel motel = new Hotel();
                motel.ID = 3;
                motel.Name = "Async Motel";
                motel.Address = "123 Front St";
                motel.Phone = "555-555-5555";

                HotelManagementService hotelService = new HotelManagementService(context);
                await context.AddAsync(hotel);
                await context.AddAsync(motel);

                await Assert.ThrowsAsync<InvalidOperationException>(async() => await hotelService.GetHotel(2));
            }
        }

        [Fact]
        public async void CanReadManyHotels()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("ReadAllHotels").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel inn = new Hotel();
                inn.ID = 1;
                inn.Name = "Async Inn";
                inn.Address = "456 Beach Rd";
                inn.Phone = "555-555-5555";

                Hotel bnb = new Hotel();
                bnb.ID = 2;
                bnb.Name = "Async B&B";
                bnb.Address = "333 Walnut Ln";
                bnb.Phone = "555-555-5555";

                Hotel motel = new Hotel();
                motel.ID = 3;
                motel.Name = "Async Motel";
                motel.Address = "123 Front St";
                motel.Phone = "555-555-5555";

                HotelManagementService hotelService = new HotelManagementService(context);
                await context.AddAsync(inn);
                await context.AddAsync(bnb);
                await context.AddAsync(motel);
                IEnumerable<Hotel> result = await hotelService.GetHotels("");

                int counter = 0;
                Hotel[] hotels = { inn, bnb, motel };
                foreach (Hotel item in result)
                {
                    Assert.Equal(item.ID, hotels[counter].ID);
                    counter++;
                }
            }
        }

        [Fact]
        public async void CanFilterHotels()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("FilterHotels").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel inn = new Hotel();
                inn.ID = 1;
                inn.Name = "Async Inn";
                inn.Address = "456 Beach Rd";
                inn.Phone = "555-555-5555";

                Hotel bnb = new Hotel();
                bnb.ID = 2;
                bnb.Name = "Async B&B";
                bnb.Address = "333 Walnut Ln";
                bnb.Phone = "555-555-5555";

                Hotel motel = new Hotel();
                motel.ID = 3;
                motel.Name = "Async Motel";
                motel.Address = "123 Front St";
                motel.Phone = "555-555-5555";

                HotelManagementService hotelService = new HotelManagementService(context);
                await context.AddAsync(inn);
                await context.AddAsync(bnb);
                await context.AddAsync(motel);
                IEnumerable<Hotel> result = await hotelService.GetHotels("Motel");

                foreach (Hotel item in result)
                {
                    Assert.Equal("Async Motel", item.Name);
                }
            }
        }

        /// <summary>
        /// Update Tests
        /// </summary>
        [Fact]
        public async void CanUpdateHotel()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("UpdateHotel").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel inn = new Hotel();
                inn.ID = 5;
                inn.Name = "Async Inn";
                inn.Address = "456 Beach Rd";
                inn.Phone = "555-555-5555";

                HotelManagementService hotelService = new HotelManagementService(context);
                await hotelService.CreateHotel(inn);

                Hotel edit = await hotelService.GetHotel(inn.ID);
                edit.Address = "111 Ocean Ln";
                await hotelService.UpdateHotel(edit);

                var result = await context.Hotels.FirstOrDefaultAsync(i => i.ID == inn.ID);
                Assert.Equal("111 Ocean Ln", result.Address);
            }
        }

        [Fact]
        public async void CannotUpdateHotel()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CannotUpdateHotel").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel inn = new Hotel();
                inn.ID = 5;
                inn.Name = "Async Inn";
                inn.Address = "456 Beach Rd";
                inn.Phone = "555-555-5555";

                HotelManagementService hotelService = new HotelManagementService(context);
                await hotelService.CreateHotel(inn);

                Hotel edit = await hotelService.GetHotel(inn.ID);
                edit.ID = 6;
                edit.Address = "111 Ocean Ln";

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await hotelService.UpdateHotel(edit));
            }
        }

        /// <summary>
        /// Delete Tests
        /// </summary>
        [Fact]
        public async void CanDeleteHotel()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("DeleteHotel").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel motel = new Hotel();
                motel.ID = 1;
                motel.Name = "Async Motel";
                motel.Address = "312 Sunset Blvd";
                motel.Phone = "555-666-7777";

                Hotel lodge = new Hotel();
                lodge.ID = 2;
                lodge.Name = "Async Lodge";
                lodge.Address = "465 Mountain Rd";
                lodge.Phone = "111-222-3333";

                HotelManagementService hotelService = new HotelManagementService(context);
                await hotelService.CreateHotel(motel);
                await hotelService.CreateHotel(lodge);

                await hotelService.DeleteHotel(motel.ID);

                Hotel result = await context.Hotels.FirstOrDefaultAsync(i => i.ID == motel.ID);
                Assert.Null(result);
            }
        }

        [Fact]
        public async void CannotDeleteHotel()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CannotDeleteHotel").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Hotel motel = new Hotel();
                motel.ID = 1;
                motel.Name = "Async Motel";
                motel.Address = "312 Sunset Blvd";
                motel.Phone = "555-666-7777";

                Hotel lodge = new Hotel();
                lodge.ID = 2;
                lodge.Name = "Async Lodge";
                lodge.Address = "465 Mountain Rd";
                lodge.Phone = "111-222-3333";

                HotelManagementService hotelService = new HotelManagementService(context);
                await hotelService.CreateHotel(motel);
                await hotelService.CreateHotel(lodge);
                
                await Assert.ThrowsAsync<ArgumentNullException>(async () => await hotelService.DeleteHotel(3));
            }
        }
    }
}
