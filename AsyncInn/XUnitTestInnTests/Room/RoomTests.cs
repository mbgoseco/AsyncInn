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
    public class RoomTests
    {
        /// <summary>
        /// Getter/Setter Tests
        /// </summary>
        [Fact]
        public void CanGetNameOfRoom()
        {
            Room room = new Room();
            room.Name = "Overnighter";

            Assert.Equal("Overnighter", room.Name);
        }

        [Fact]
        public void CanSetNameOfRoom()
        {
            Room room = new Room();
            room.Name = "Deluxe";
            room.Name = "Beef Supreme";

            Assert.Equal("Beef Supreme", room.Name);
        }

        [Fact]
        public void CanGetLayoutOfRoom()
        {
            Room room = new Room();
            room.Layout = Layouts.Studio;

            Assert.Equal(Layouts.Studio, room.Layout);
        }

        [Fact]
        public void CanSetLayoutOfRoom()
        {
            Room room = new Room();
            room.Layout = Layouts.Studio;
            room.Layout = Layouts.TwoBedroom;

            Assert.Equal(Layouts.TwoBedroom, room.Layout);
        }

        /// <summary>
        /// Create Tests
        /// </summary>
        [Fact]
        public async void CanCreateRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreateRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room vista = new Room();
                vista.ID = 1;
                vista.Name = "Vista";
                vista.Layout = Layouts.OneBedroom;

                RoomManagementService roomService = new RoomManagementService(context);
                await roomService.CreateRoom(vista);

                Room result = await context.Rooms.FirstOrDefaultAsync(h => h.ID == vista.ID);

                Assert.Equal(vista, result);
            }
        }

        [Fact]
        public async void CreatedEmptyRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreatedEmptyRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room ritz = new Room();
                ritz.ID = 1;

                RoomManagementService roomService = new RoomManagementService(context);
                await roomService.CreateRoom(ritz);

                Room result = await context.Rooms.FirstOrDefaultAsync(h => h.Name != null);

                Assert.NotEqual(ritz, result);
            }
        }

        /// <summary>
        /// Read Tests
        /// </summary>
        [Fact]
        public async void CanReadIndividualRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("ReadSingleRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room deluxe = new Room();
                deluxe.ID = 1;
                deluxe.Name = "Deluxe";
                deluxe.Layout = Layouts.TwoBedroom;

                RoomManagementService roomService = new RoomManagementService(context);
                await roomService.CreateRoom(deluxe);

                Room result = await roomService.GetRoom(1);

                Assert.Equal(deluxe, result);
            }
        }

        [Fact]
        public async void CannotReadMissingRoomID()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CantReadMissingRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room mini = new Room();
                mini.ID = 1;
                mini.Name = "Mini";
                mini.Layout = Layouts.Studio;

                Room single = new Room();
                single.ID = 3;
                single.Name = "Single";
                single.Layout = Layouts.OneBedroom;

                RoomManagementService roomService = new RoomManagementService(context);
                await context.AddAsync(mini);
                await context.AddAsync(single);;

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await roomService.GetRoom(2));
            }
        }

        [Fact]
        public async void CanReadManyRooms()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("ReadAllRooms").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room mini = new Room();
                mini.ID = 1;
                mini.Name = "Mini";
                mini.Layout = Layouts.Studio;

                Room single = new Room();
                single.ID = 2;
                single.Name = "Single";
                single.Layout = Layouts.OneBedroom;

                Room supreme = new Room();
                supreme.ID = 3;
                supreme.Name = "Supreme";
                supreme.Layout = Layouts.TwoBedroom;

                RoomManagementService roomService = new RoomManagementService(context);
                await roomService.CreateRoom(mini);
                await roomService.CreateRoom(single);
                await roomService.CreateRoom(supreme);
                IEnumerable<Room> result = await roomService.GetRooms("");
                List<Room> list = new List<Room>();
                foreach (Room item in result)
                {
                    list.Add(item);
                }

                Assert.Equal(3, list.Count);
            }
        }

        [Fact]
        public async void CanFilterRooms()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("FilterRooms").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room mini = new Room();
                mini.ID = 1;
                mini.Name = "Mini";
                mini.Layout = Layouts.Studio;

                Room single = new Room();
                single.ID = 2;
                single.Name = "Single";
                single.Layout = Layouts.OneBedroom;

                Room supreme = new Room();
                supreme.ID = 3;
                supreme.Name = "Supreme";
                supreme.Layout = Layouts.TwoBedroom;

                RoomManagementService roomService = new RoomManagementService(context);
                await roomService.CreateRoom(mini);
                await roomService.CreateRoom(single);
                await roomService.CreateRoom(supreme);
                IEnumerable<Room> result = await roomService.GetRooms("Supreme");
                Room match = new Room();
                foreach (Room item in result)
                {
                    match = item;
                }

                Assert.Equal(supreme, match);
            }
        }

        /// <summary>
        /// Update Tests
        /// </summary>
        [Fact]
        public async void CanUpdateRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("UpdateRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room ritz = new Room();
                ritz.ID = 1;
                ritz.Name = "Ritz";
                ritz.Layout = Layouts.Studio;

                RoomManagementService roomService = new RoomManagementService(context);
                await roomService.CreateRoom(ritz);

                Room edit = await roomService.GetRoom(ritz.ID);
                edit.Layout = Layouts.OneBedroom;
                await roomService.UpdateRoom(edit);

                var result = await context.Rooms.FirstOrDefaultAsync(i => i.ID == ritz.ID);
                Assert.Equal(Layouts.OneBedroom, result.Layout);
            }
        }

        [Fact]
        public async void CannotUpdateRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CannotUpdateRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room ritz = new Room();
                ritz.ID = 1;
                ritz.Name = "Ritz";
                ritz.Layout = Layouts.Studio;

                RoomManagementService roomService = new RoomManagementService(context);
                await roomService.CreateRoom(ritz);

                Room edit = await roomService.GetRoom(ritz.ID);
                edit.ID = 2;
                edit.Name = "Flophouse";
                edit.Layout = Layouts.OneBedroom;

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await roomService.UpdateRoom(edit));
            }
        }

        /// <summary>
        /// Delete Tests
        /// </summary>
        [Fact]
        public async void CanDeleteRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("DeleteRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room honeymoon = new Room();
                honeymoon.ID = 1;
                honeymoon.Name = "Honeymoon";
                honeymoon.Layout = Layouts.OneBedroom;

                Room apline = new Room();
                apline.ID = 2;
                apline.Name = "Apline";
                apline.Layout = Layouts.TwoBedroom;

                RoomManagementService roomService = new RoomManagementService(context);
                await roomService.CreateRoom(honeymoon);
                await roomService.CreateRoom(apline);

                await roomService.DeleteRoom(honeymoon.ID);

                Room result = await context.Rooms.FirstOrDefaultAsync(i => i.ID == honeymoon.ID);
                Assert.Null(result);
            }
        }

        [Fact]
        public async void CannotDeleteRoom()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CannotDeleteRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Room honeymoon = new Room();
                honeymoon.ID = 1;
                honeymoon.Name = "Honeymoon";
                honeymoon.Layout = Layouts.OneBedroom;

                Room apline = new Room();
                apline.ID = 2;
                apline.Name = "Apline";
                apline.Layout = Layouts.TwoBedroom;

                RoomManagementService roomService = new RoomManagementService(context);
                await roomService.CreateRoom(honeymoon);
                await roomService.CreateRoom(apline);

                await Assert.ThrowsAsync<ArgumentNullException>(async () => await roomService.DeleteRoom(3));
            }
        }
    }
}
