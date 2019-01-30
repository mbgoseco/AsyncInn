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

        public async Task CreateAmenities(Amenities amenities)
        {
            _context.Amenities.Add(amenities);
            await _context.SaveChangesAsync();
        }

        public async Task<Amenities> GetAmenities(int id)
        {
            return await _context.Amenities.FirstOrDefaultAsync(amenities => amenities.ID == id);
        }

        public async Task<IEnumerable<Amenities>> GetAmenities()
        {
            return await _context.Amenities.ToListAsync();
        }

        public async Task UpdateAmenities(Amenities amenities)
        {
            _context.Amenities.Update(amenities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAmenities(int id)
        {
            Amenities amenities = _context.Amenities.FirstOrDefault(a => a.ID == id);
            _context.Amenities.Remove(amenities);
            await _context.SaveChangesAsync();
        }
    }
}
