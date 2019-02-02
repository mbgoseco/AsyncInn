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
        /// Calls the Add method from Entity Framework to add the given form data to the Hotel database.
        /// </summary>
        /// <param name="hotel">Form data passed in from the Create view</param>
        /// <returns>New row to the Hotel table of the database</returns>
        public async Task CreateHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
        }
        
        public async Task<Hotel> GetHotel(int id)
        {
            var hotels = await _context.Hotels.FirstOrDefaultAsync(hotel => hotel.ID == id);

            hotels.Rooms = await _context.HotelRooms.Where(r => r.HotelID == hotels.ID).ToListAsync();

            return hotels;
        }

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

        public async Task UpdateHotel(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHotel(int id)
        {
            Hotel hotel = _context.Hotels.FirstOrDefault(h => h.ID == id);
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
        }
    }
}
