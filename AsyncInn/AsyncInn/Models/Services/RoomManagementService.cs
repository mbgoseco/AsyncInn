using AsyncInn.Data;
using AsyncInn.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Services
{
    public class RoomManagementService : IRoom
    {
        private AsyncInnDbContext _context { get; }

        public RoomManagementService(AsyncInnDbContext context)
        {
            _context = context;
        }

        public async Task CreateRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }

        public async Task<Room> GetRoom(int id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(room => room.ID == id);
        }

        public async Task<IEnumerable<Room>> GetRooms(string searchRooms)
        {
            var rooms = from m in _context.Rooms
                         select m;

            if (!String.IsNullOrEmpty(searchRooms))
            {
                rooms = rooms.Where(r => r.Name.Contains(searchRooms));
            }

            return await rooms.ToListAsync();
        }

        public async Task UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoom(int id)
        {
            Room room = _context.Rooms.FirstOrDefault(r => r.ID == id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }
}
