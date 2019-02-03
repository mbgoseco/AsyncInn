using AsyncInn.Data;
using AsyncInn.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Services
{
    public class AmenitiesManagementService : IAmenities
    {
        private AsyncInnDbContext _context { get; }

        public AmenitiesManagementService(AsyncInnDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Takes in data from the Create view form which was validated through the controller and creates a new Amenities entry in the table.
        /// </summary>
        /// <param name="amenities">New amenity data from form</param>
        /// <returns>New entry to the Amenities table</returns>
        public async Task CreateAmenities(Amenities amenities)
        {
            _context.Amenities.Add(amenities);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Selects a single Amenities entry to display for the Edit, Details, or Delete views
        /// </summary>
        /// <param name="id">Selected amenity ID</param>
        /// <returns>Selected amenity</returns>
        public async Task<Amenities> GetAmenities(int id)
        {
            return await _context.Amenities.FirstOrDefaultAsync(amenities => amenities.ID == id);
        }

        /// <summary>
        /// Displays all amenities to the Index view or, if a search string is passed to it, displays all entries that match the search criteria.
        /// </summary>
        /// <param name="searchAmenities">Search criteria by name</param>
        /// <returns>All amenites or filtered amenities</returns>
        public async Task<IEnumerable<Amenities>> GetAmenities(string searchAmenities)
        {
            var amenities = from s in _context.Amenities
                         select s;

            if (!String.IsNullOrEmpty(searchAmenities))
            {
                amenities = amenities.Where(a => a.Name.Contains(searchAmenities));
            }

            return await amenities.ToListAsync();
        }

        /// <summary>
        /// Updates an existing Amenities entry with data from the Edit view form, verified by the controller
        /// </summary>
        /// <param name="amenities">Edited amenity data from form</param>
        /// <returns>List of amenities with updated entry</returns>
        public async Task UpdateAmenities(Amenities amenities)
        {
            _context.Amenities.Update(amenities);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a selected Amenities entry from the table
        /// </summary>
        /// <param name="id">Selected amenity ID</param>
        /// <returns>List of amenities minus the deleted one</returns>
        public async Task DeleteAmenities(int id)
        {
            Amenities amenities = _context.Amenities.FirstOrDefault(a => a.ID == id);
            _context.Amenities.Remove(amenities);
            await _context.SaveChangesAsync();
        }
    }
}
