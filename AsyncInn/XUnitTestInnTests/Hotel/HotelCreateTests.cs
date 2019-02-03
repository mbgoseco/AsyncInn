using System;
using Xunit;
using AsyncInn.Models;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models.Services;

namespace XUnitTestInnTests
{
    public class HotelCreateTests
    {
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
    }
}
