using System;
using Xunit;
using AsyncInn.Models;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models.Services;
using System.Collections.Generic;
using AsyncInn.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;

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
                await roomController.Create(hRoom);

                HotelRoom result = await context.HotelRooms.FirstOrDefaultAsync(h => h.HotelID == hRoom.HotelID && h.RoomNumberID == hRoom.RoomNumberID);

                Assert.Equal(hRoom, result);
            }
        }

        [Fact]
        public async void CreatedEmptyHotelRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreatedEmptyHotelRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                HotelRoom hRoom = new HotelRoom();
                hRoom.HotelID = 1;

                HotelRoomController roomController = new HotelRoomController(context);
                await roomController.Create(hRoom);

                HotelRoom result = await context.HotelRooms.FirstOrDefaultAsync(h => h.RoomNumberID != 0);

                Assert.NotEqual(hRoom, result);
            }
        }

        [Fact]
        public async void CannotCreateDuplicateHotelRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreatedEmptyHotelRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                HotelRoom hRoom = new HotelRoom();
                hRoom.HotelID = 1;
                hRoom.RoomNumberID = 101;
                hRoom.RoomID = 3;
                hRoom.Rate = 39.89M;
                hRoom.PetFriendly = true;

                HotelRoom dupRoom = new HotelRoom();
                dupRoom.HotelID = 1;
                dupRoom.RoomNumberID = 101;
                dupRoom.RoomID = 2;
                dupRoom.Rate = 42.99M;
                dupRoom.PetFriendly = false;

                HotelRoomController roomController = new HotelRoomController(context);
                await roomController.Create(hRoom);
                await roomController.Create(dupRoom);

                HotelRoom result = await context.HotelRooms.FirstOrDefaultAsync(h => h.RoomID == 2);

                Assert.Null(result);
            }
        }

        /// <summary>
        /// Read Tests
        /// </summary>
        [Fact]
        public async void IndexReturnsViewOfHotelRooms()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("IndexReturnsView").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                HotelRoom hRoom = new HotelRoom();
                hRoom.HotelID = 3;
                hRoom.RoomNumberID = 400;
                hRoom.RoomID = 2;
                hRoom.Rate = 75.00M;
                hRoom.PetFriendly = false;

                HotelRoom iRoom = new HotelRoom();
                iRoom.HotelID = 2;
                iRoom.RoomNumberID = 324;
                iRoom.RoomID = 1;
                iRoom.Rate = 55.09M;
                iRoom.PetFriendly = true;

                HotelRoom jRoom = new HotelRoom();
                jRoom.HotelID = 5;
                jRoom.RoomNumberID = 211;
                jRoom.RoomID = 3;
                jRoom.Rate = 49.89M;
                jRoom.PetFriendly = true;

                HotelRoomController roomController = new HotelRoomController(context);
                await roomController.Create(hRoom);
                await roomController.Create(iRoom);
                await roomController.Create(jRoom);

                Assert.IsType<ViewResult>(await roomController.Index());
            }
        }

        [Fact]
        public async void IndexReturnsListOfHotelRooms()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("IndexReturnsList").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                HotelRoom hRoom = new HotelRoom();
                hRoom.HotelID = 3;
                hRoom.RoomNumberID = 400;
                hRoom.RoomID = 2;
                hRoom.Rate = 75.00M;
                hRoom.PetFriendly = false;

                HotelRoom iRoom = new HotelRoom();
                iRoom.HotelID = 2;
                iRoom.RoomNumberID = 324;
                iRoom.RoomID = 1;
                iRoom.Rate = 55.09M;
                iRoom.PetFriendly = true;

                HotelRoom jRoom = new HotelRoom();
                jRoom.HotelID = 5;
                jRoom.RoomNumberID = 211;
                jRoom.RoomID = 3;
                jRoom.Rate = 49.89M;
                jRoom.PetFriendly = true;

                HotelRoomController roomController = new HotelRoomController(context);
                await roomController.Create(hRoom);
                await roomController.Create(iRoom);
                await roomController.Create(jRoom);

                var result = await context.HotelRooms.Where(r => r.RoomNumberID != 0).ToListAsync();
                Assert.Equal(3, result.Count);
            }
        }

        /// <summary>
        /// Update Tests
        /// </summary>
        [Fact]
        public async void CanUpdateHotelRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("UpdateHotelRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                HotelRoom hRoom = new HotelRoom();
                hRoom.HotelID = 3;
                hRoom.RoomNumberID = 400;
                hRoom.RoomID = 2;
                hRoom.Rate = 75.00M;
                hRoom.PetFriendly = false;

                HotelRoomController roomController = new HotelRoomController(context);
                await roomController.Create(hRoom);

                HotelRoom edit = hRoom;
                edit.HotelID = 3;
                edit.RoomNumberID = 400;
                edit.RoomID = 2;
                edit.Rate = 65.99M;
                edit.PetFriendly = false;
                await roomController.Edit(400, 3, edit);

                var result = await context.HotelRooms.FirstOrDefaultAsync(i => i.Rate == hRoom.Rate);
                Assert.Equal(65.99M, result.Rate);
            }
        }

        [Fact]
        public async void CannotUpdateIntoDuplicateHotelRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("NoUpdateDupesHotelRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                HotelRoom hRoom = new HotelRoom();
                hRoom.HotelID = 3;
                hRoom.RoomNumberID = 400;
                hRoom.RoomID = 2;
                hRoom.Rate = 75.00M;
                hRoom.PetFriendly = false;

                HotelRoom iRoom = new HotelRoom();
                iRoom.HotelID = 5;
                iRoom.RoomNumberID = 229;
                iRoom.RoomID = 1;
                iRoom.Rate = 55.00M;
                iRoom.PetFriendly = true;

                HotelRoomController roomController = new HotelRoomController(context);
                await roomController.Create(hRoom);
                await roomController.Create(iRoom);

                HotelRoom edit = iRoom;
                edit.HotelID = 3;
                edit.RoomNumberID = 400;
                edit.RoomID = 1;
                edit.Rate = 55.00M;
                edit.PetFriendly = true;

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await roomController.Edit(edit.RoomNumberID, edit.HotelID, edit));
            }
        }

        /// <summary>
        /// Delete Tests
        /// </summary>
        [Fact]
        public async void CanDeleteHotelRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("DeleteHotelRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                HotelRoom hRoom = new HotelRoom();
                hRoom.HotelID = 3;
                hRoom.RoomNumberID = 400;
                hRoom.RoomID = 2;
                hRoom.Rate = 75.00M;
                hRoom.PetFriendly = false;

                HotelRoom iRoom = new HotelRoom();
                iRoom.HotelID = 5;
                iRoom.RoomNumberID = 229;
                iRoom.RoomID = 1;
                iRoom.Rate = 55.00M;
                iRoom.PetFriendly = true;

                HotelRoomController roomController = new HotelRoomController(context);
                await roomController.Create(hRoom);
                await roomController.Create(iRoom);

                await roomController.DeleteConfirmed(400, 3);

                HotelRoom result = await context.HotelRooms.FirstOrDefaultAsync(i => i.HotelID == 3);
                Assert.Null(result);
            }
        }

        [Fact]
        public async void CannotDeleteHotelRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CannotDeleteHotelRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                HotelRoom hRoom = new HotelRoom();
                hRoom.HotelID = 3;
                hRoom.RoomNumberID = 400;
                hRoom.RoomID = 2;
                hRoom.Rate = 75.00M;
                hRoom.PetFriendly = false;

                HotelRoom iRoom = new HotelRoom();
                iRoom.HotelID = 5;
                iRoom.RoomNumberID = 229;
                iRoom.RoomID = 1;
                iRoom.Rate = 55.00M;
                iRoom.PetFriendly = true;

                HotelRoomController roomController = new HotelRoomController(context);
                await roomController.Create(hRoom);
                await roomController.Create(iRoom);

                await Assert.ThrowsAsync<ArgumentNullException>(async () => await roomController.DeleteConfirmed(115, 4));
            }
        }
    }
}
