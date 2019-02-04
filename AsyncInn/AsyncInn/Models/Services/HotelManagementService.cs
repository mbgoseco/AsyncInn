using AsyncInn.Data;
using AsyncInn.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Services
{
    public class HotelManagementService : IHotel
    {
        private AsyncInnDbContext _context { get; }

        public HotelManagementService(AsyncInnDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Takes in data from the Create view form which was validated through the controller and creates a new Hotel entry in the table.
        /// </summary>
        /// <param name="hotel">Form data passed in from the Create view</param>
        /// <returns>New entry to the Hotel table</returns>
        public async Task CreateHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Selects a single Amenities entry to display for the Edit, Details, or Delete views
        /// </summary>
        /// <param name="id">Selected hotel ID</param>
        /// <returns>Selected hotel</returns>
        public async Task<Hotel> GetHotel(int id)
        {
            var hotels = await _context.Hotels.FirstOrDefaultAsync(hotel => hotel.ID == id);

            hotels.Rooms = await _context.HotelRooms.Where(r => r.HotelID == hotels.ID).ToListAsync();

            return hotels;
        }

        /// <summary>
        /// Displays all hotels to the Index view or, if a search string is passed to it, displays all entries that match the search criteria.
        /// </summary>
        /// <param name="searchHotels">Search criteria by name</param>
        /// <returns>All or filtered hotels</returns>
        public async Task<IEnumerable<Hotel>> GetHotels(string searchHotels)
        {
            var hotels = from h in _context.Hotels
                         select h;

            if (!String.IsNullOrEmpty(searchHotels))
            {
                hotels = hotels.Where(s => s.Name.Contains(searchHotels));
            }

            foreach (Hotel hotel in hotels)
            {
                hotel.Rooms = await _context.HotelRooms.Where(r => r.HotelID == hotel.ID).ToListAsync();
            }

            return await hotels.ToListAsync();
        }

        /// <summary>
        /// Updates an existing Hotel entry with data from the Edit view form, verified by the controller
        /// </summary>
        /// <param name="hotel">Edited hotel data from form</param>
        /// <returns>List of hotels with updated entry</returns>
        public async Task UpdateHotel(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a selected Hotel entry from the table, and also deletes any HotelRoom table entries with the same hotel ID.
        /// </summary>
        /// <param name="id">Selected hotel ID</param>
        /// <returns>List of hotels minus the deleted one</returns>
        public async Task DeleteHotel(int id)
        {
            Hotel hotel = _context.Hotels.FirstOrDefault(h => h.ID == id);
            _context.Hotels.Remove(hotel);

            List<HotelRoom> rooms = _context.HotelRooms.Where(r => r.HotelID == hotel.ID).ToList();
            foreach(HotelRoom room in rooms)
            {
                _context.HotelRooms.Remove(room);
            }

            await _context.SaveChangesAsync();
        }
    }
}
