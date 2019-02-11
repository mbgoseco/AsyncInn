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
    public class RoomAmenitiesTests
    {
        /// <summary>
        /// Getter/Setter Tests
        /// </summary>
        [Fact]
        public void CanGetAmenitiesID()
        {
            RoomAmenities rAmen = new RoomAmenities();
            rAmen.AmenitiesID = 1;

            Assert.Equal(1, rAmen.AmenitiesID);
        }

        [Fact]
        public void CanSetAmenitiesID()
        {
            RoomAmenities rAmen = new RoomAmenities();
            rAmen.AmenitiesID = 1;
            rAmen.AmenitiesID = 2;

            Assert.Equal(2, rAmen.AmenitiesID);
        }

        [Fact]
        public void CanGetRoomID()
        {
            RoomAmenities rAmen = new RoomAmenities();
            rAmen.RoomID = 1;

            Assert.Equal(1, rAmen.RoomID);
        }

        [Fact]
        public void CanSetRoomID()
        {
            RoomAmenities rAmen = new RoomAmenities();
            rAmen.RoomID = 1;
            rAmen.RoomID = 2;

            Assert.Equal(2, rAmen.RoomID);
        }

        /// <summary>
        /// Create Tests
        /// </summary>
        [Fact]
        public async void CanCreateRoomAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreateRoomAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                RoomAmenities rAmen = new RoomAmenities();
                rAmen.AmenitiesID = 2;
                rAmen.RoomID = 4;

                RoomAmenitiesController ramenController = new RoomAmenitiesController(context);
                await ramenController.Create(rAmen);

                RoomAmenities result = await context.RoomAmenities.FirstOrDefaultAsync(r => r.RoomID == rAmen.RoomID);

                Assert.Equal(rAmen, result);
            }
        }

        [Fact]
        public async void CreatedEmptyRoomAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreatedEmptyHotelRoom").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                RoomAmenities rAmen = new RoomAmenities();

                RoomAmenitiesController ramenController = new RoomAmenitiesController(context);
                await ramenController.Create(rAmen);

                RoomAmenities result = await context.RoomAmenities.FirstOrDefaultAsync(r => r.RoomID != 1);

                Assert.NotEqual(rAmen, result);
            }
        }

        /// <summary>
        /// Read Tests
        /// </summary>
        [Fact]
        public async void IndexReturnsViewOfRoomAmenities()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("IndexReturnsRoomAmenitiesView").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                RoomAmenities rAmenA = new RoomAmenities();
                rAmenA.AmenitiesID = 2;
                rAmenA.RoomID = 4;

                RoomAmenities rAmenB = new RoomAmenities();
                rAmenB.AmenitiesID = 1;
                rAmenB.RoomID = 3;

                RoomAmenities rAmenC = new RoomAmenities();
                rAmenC.AmenitiesID = 4;
                rAmenC.RoomID = 5;

                RoomAmenitiesController ramenController = new RoomAmenitiesController(context);
                await ramenController.Create(rAmenA);
                await ramenController.Create(rAmenB);
                await ramenController.Create(rAmenC);

                Assert.IsType<ViewResult>(await ramenController.Index());
            }
        }

        [Fact]
        public async void IndexReturnsListOfRoomAmenities()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("IndexReturnsRoomAmenitiesList").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                RoomAmenities rAmenA = new RoomAmenities();
                rAmenA.AmenitiesID = 2;
                rAmenA.RoomID = 4;

                RoomAmenities rAmenB = new RoomAmenities();
                rAmenB.AmenitiesID = 1;
                rAmenB.RoomID = 3;

                RoomAmenities rAmenC = new RoomAmenities();
                rAmenC.AmenitiesID = 4;
                rAmenC.RoomID = 5;

                RoomAmenitiesController ramenController = new RoomAmenitiesController(context);
                await ramenController.Create(rAmenA);
                await ramenController.Create(rAmenB);
                await ramenController.Create(rAmenC);

                var result = await context.RoomAmenities.ToListAsync();
                Assert.Equal(3, result.Count);
            }
        }

        /// <summary>
        /// Delete Tests
        /// </summary>
        [Fact]
        public async void CanDeleteRoomAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("DeleteRoomAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                RoomAmenities rAmenA = new RoomAmenities();
                rAmenA.AmenitiesID = 2;
                rAmenA.RoomID = 4;

                RoomAmenities rAmenB = new RoomAmenities();
                rAmenB.AmenitiesID = 1;
                rAmenB.RoomID = 3;

                RoomAmenities rAmenC = new RoomAmenities();
                rAmenC.AmenitiesID = 4;
                rAmenC.RoomID = 5;

                RoomAmenitiesController ramenController = new RoomAmenitiesController(context);
                await ramenController.Create(rAmenA);
                await ramenController.Create(rAmenB);
                await ramenController.Create(rAmenC);

                await ramenController.DeleteConfirmed(1, 3);

                RoomAmenities result = await context.RoomAmenities.FirstOrDefaultAsync(i => i.AmenitiesID == 1 && i.RoomID == 3);
                Assert.Null(result);
            }
        }

        [Fact]
        public async void CannotDeleteRoomAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CannotDeleteRoomAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                RoomAmenities rAmenA = new RoomAmenities();
                rAmenA.AmenitiesID = 2;
                rAmenA.RoomID = 4;

                RoomAmenities rAmenB = new RoomAmenities();
                rAmenB.AmenitiesID = 1;
                rAmenB.RoomID = 3;

                RoomAmenities rAmenC = new RoomAmenities();
                rAmenC.AmenitiesID = 4;
                rAmenC.RoomID = 5;

                RoomAmenitiesController ramenController = new RoomAmenitiesController(context);
                await ramenController.Create(rAmenA);
                await ramenController.Create(rAmenB);
                await ramenController.Create(rAmenC);

                await ramenController.DeleteConfirmed(1, 3);

                await Assert.ThrowsAsync<ArgumentNullException>(async () => await ramenController.DeleteConfirmed(9, 9));
            }
        }
    }
}
