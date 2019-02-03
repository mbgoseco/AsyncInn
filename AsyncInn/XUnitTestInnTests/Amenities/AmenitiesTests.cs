using System;
using Xunit;
using AsyncInn.Models;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models.Services;
using System.Collections.Generic;

namespace XUnitTestInnTests
{
    public class AmenitiesTests
    {
        /// <summary>
        /// Getter/Setter Tests
        /// </summary>
        [Fact]
        public void CanGetNameOfAmenity()
        {
            Amenities amenity = new Amenities();
            amenity.Name = "Hot Tub";

            Assert.Equal("Hot Tub", amenity.Name);
        }

        [Fact]
        public void CanSetNameOfAmenity()
        {
            Amenities amenity = new Amenities();
            amenity.Name = "Hot Tub";
            amenity.Name = "Mini Bar";

            Assert.Equal("Mini Bar", amenity.Name);
        }

        /// <summary>
        /// Create Tests
        /// </summary>
        [Fact]
        public async void CanCreateAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreateAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities amenity = new Amenities();
                amenity.ID = 1;
                amenity.Name = "Coffee Maker";

                AmenitiesManagementService amenitiesService = new AmenitiesManagementService(context);
                await amenitiesService.CreateAmenities(amenity);

                Amenities result = await context.Amenities.FirstOrDefaultAsync(h => h.ID == amenity.ID);

                Assert.Equal(amenity, result);
            }
        }

        [Fact]
        public async void CreatedEmptyAmenityName()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CreateEmptyAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities amenity = new Amenities();
                amenity.ID = 1;

                AmenitiesManagementService amenitiesService = new AmenitiesManagementService(context);
                await amenitiesService.CreateAmenities(amenity);

                Amenities result = await context.Amenities.FirstOrDefaultAsync(h => h.Name != null);

                Assert.NotEqual(amenity, result);
            }
        }

        /// <summary>
        /// Read Tests
        /// </summary>
        [Fact]
        public async void CanReadIndividualAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("ReadSingleAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities amenity = new Amenities();
                amenity.ID = 1;
                amenity.Name = "Free Wi-Fi";

                AmenitiesManagementService amenitiesService = new AmenitiesManagementService(context);
                await amenitiesService.CreateAmenities(amenity);

                Amenities result = await amenitiesService.GetAmenities(1);

                Assert.Equal(amenity, result);
            }
        }

        [Fact]
        public async void CannotReadMissingAmenitiesID()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CantReadMissingAmenities").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities amenityOne = new Amenities();
                amenityOne.ID = 1;
                amenityOne.Name = "Kitchen";

                Amenities amenityTwo = new Amenities();
                amenityTwo.ID = 3;
                amenityTwo.Name = "Hot Tub";

                AmenitiesManagementService amenitiesService = new AmenitiesManagementService(context);
                await context.AddAsync(amenityOne);
                await context.AddAsync(amenityTwo);

                Amenities result = await amenitiesService.GetAmenities(2);

                Assert.Null(result);
            }
        }


        [Fact]
        public async void CanReadManyAmenities()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("ReadAllAmenities").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities hotTub = new Amenities();
                hotTub.ID = 1;
                hotTub.Name = "Hot Tub";

                Amenities miniBar = new Amenities();
                miniBar.ID = 2;
                miniBar.Name = "Mini Bar";

                Amenities WiFi = new Amenities();
                WiFi.ID = 3;
                WiFi.Name = "Wi-Fi";

                AmenitiesManagementService amenitiesService = new AmenitiesManagementService(context);
                await context.AddAsync(hotTub);
                await context.AddAsync(miniBar);
                await context.AddAsync(WiFi);
                IEnumerable<Amenities> result = await amenitiesService.GetAmenities("");

                int counter = 0;
                Amenities[] amenities = { hotTub, miniBar, WiFi };
                foreach (Amenities item in result)
                {
                    Assert.Equal(item.ID, amenities[counter].ID);
                    counter++;
                }
            }
        }

        [Fact]
        public async void CanFilterAmenities()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("FilterAmenities").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities hotTub = new Amenities();
                hotTub.ID = 1;
                hotTub.Name = "Hot Tub";

                Amenities miniBar = new Amenities();
                miniBar.ID = 2;
                miniBar.Name = "Mini Bar";

                Amenities WiFi = new Amenities();
                WiFi.ID = 3;
                WiFi.Name = "Wi-Fi";

                AmenitiesManagementService amenitiesService = new AmenitiesManagementService(context);
                await context.AddAsync(hotTub);
                await context.AddAsync(miniBar);
                await context.AddAsync(WiFi);
                IEnumerable<Amenities> result = await amenitiesService.GetAmenities("Wi-Fi");

                foreach (Amenities item in result)
                {
                    Assert.Equal(WiFi, item);
                }
            }
        }

        /// <summary>
        /// Update Tests
        /// </summary>
        [Fact]
        public async void CanUpdateAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("UpdateAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities kitchen = new Amenities();
                kitchen.ID = 1;
                kitchen.Name = "Kitchen";

                AmenitiesManagementService AmenitiesService = new AmenitiesManagementService(context);
                await AmenitiesService.CreateAmenities(kitchen);

                Amenities edit = await AmenitiesService.GetAmenities(kitchen.ID);
                edit.Name = "Free Beer";
                await AmenitiesService.UpdateAmenities(edit);

                var result = await context.Amenities.FirstOrDefaultAsync(i => i.ID == kitchen.ID);
                Assert.Equal("Free Beer", result.Name);
            }
        }

        [Fact]
        public async void CannotUpdateAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CannotUpdateAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities kitchen = new Amenities();
                kitchen.ID = 1;
                kitchen.Name = "Kitchen";

                AmenitiesManagementService amenitiesService = new AmenitiesManagementService(context);
                await amenitiesService.CreateAmenities(kitchen);

                Amenities edit = await amenitiesService.GetAmenities(kitchen.ID);
                edit.ID = 2;
                edit.Name = "Free Beer";

                await Assert.ThrowsAsync<InvalidOperationException>(async () => await amenitiesService.UpdateAmenities(edit));
            }
        }

        /// <summary>
        /// Delete Tests
        /// </summary>
        [Fact]
        public async void CanDeleteAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("DeleteAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities patio = new Amenities();
                patio.ID = 1;
                patio.Name = "Patio";

                Amenities beach = new Amenities();
                beach.ID = 2;
                beach.Name = "Beach Access";

                AmenitiesManagementService AmenitiesService = new AmenitiesManagementService(context);
                await AmenitiesService.CreateAmenities(patio);
                await AmenitiesService.CreateAmenities(beach);

                await AmenitiesService.DeleteAmenities(patio.ID);

                Amenities result = await context.Amenities.FirstOrDefaultAsync(i => i.ID == patio.ID);
                Assert.Null(result);
            }
        }

        [Fact]
        public async void CannotDeleteAmenity()
        {
            DbContextOptions<AsyncInnDbContext> options = new DbContextOptionsBuilder<AsyncInnDbContext>().UseInMemoryDatabase("CannotDeleteAmenity").Options;

            using (AsyncInnDbContext context = new AsyncInnDbContext(options))
            {
                Amenities patio = new Amenities();
                patio.ID = 1;
                patio.Name = "Patio";

                Amenities beach = new Amenities();
                beach.ID = 2;
                beach.Name = "Beach Access";

                AmenitiesManagementService amenitiesService = new AmenitiesManagementService(context);
                await amenitiesService.CreateAmenities(patio);
                await amenitiesService.CreateAmenities(beach);

                await Assert.ThrowsAsync<ArgumentNullException>(async () => await amenitiesService.DeleteAmenities(3));
            }
        }
    }
}
