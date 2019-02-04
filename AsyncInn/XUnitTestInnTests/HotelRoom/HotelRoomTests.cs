using System;
using Xunit;
using AsyncInn.Models;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models.Services;
using System.Collections.Generic;
using AsyncInn.Controllers;

namespace XUnitTestInnTests
{
    public class HotelRoomTests
    {
        /// <summary>
        /// Getter/Setter Tests
        /// </summary>
        [Fact]
        public void CanGetRateOfHotelRoom()
        {
            HotelRoom hRoom = new HotelRoom();
            hRoom.Rate = 49.99M;

            Assert.Equal(49.99M, hRoom.Rate);
        }

        [Fact]
        public void CanSetPetFriendlinessOfHotelRoom()
        {
            HotelRoom hRoom = new HotelRoom();
            hRoom.PetFriendly = true;

            Assert.True(hRoom.PetFriendly);
        }

        /// <summary>
        /// Create Tests
        /// </summary>
        [Fact]
        public async void CanCreateHotelRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreateRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                HotelRoom hRoom = new HotelRoom();
                hRoom.HotelID = 1;
                hRoom.RoomNumberID = 101;
                hRoom.RoomID = 3;
                hRoom.Rate = 39.89M;
                hRoom.PetFriendly = true;

                HotelRoomController roomController = new HotelRoomController(context);
                await context.HotelRooms.AddAsync(hRoom);

                HotelRoom result = await context.HotelRooms.FirstOrDefaultAsync(h => h.HotelID == hRoom.HotelID && h.RoomNumberID == hRoom.RoomNumberID);

                Assert.Equal(hRoom, result);
            }
        }
    }
}
