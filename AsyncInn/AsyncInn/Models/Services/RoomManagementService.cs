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

        /// <summary>
        /// Takes in data from the create view form which was validated through the controller and creates a new Room entry in the table.
        /// </summary>
        /// <param name="room">New room data from form</param>
        /// <returns>New entry to the Room table</returns>
        public async Task CreateRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Selects a single Room entry to display for the Edit, Details, or Delete views
        /// </summary>
        /// <param name="id">Selected room ID</param>
        /// <returns>Selected room</returns>
        public async Task<Room> GetRoom(int id)
        {
            var rooms = await _context.Rooms.FirstOrDefaultAsync(room => room.ID == id);

            rooms.RoomID = await _context.RoomAmenities.Where(a => a.RoomID == rooms.ID).ToListAsync();

            return rooms;
        }

        /// <summary>
        /// Displays all rooms to the Index view or, if a search string is passed to it, displays all entries that match the search criteria.
        /// </summary>
        /// <param name="searchHotels">Search criteria by name</param>
        /// <returns>All or filtered rooms</returns>
        public async Task<IEnumerable<Room>> GetRooms(string searchRooms)
        {
            var rooms = from m in _context.Rooms
                         select m;

            if (!String.IsNullOrEmpty(searchRooms))
            {
                rooms = rooms.Where(r => r.Name.Contains(searchRooms));
            }

            foreach (Room room in rooms)
            {
                room.RoomID = await _context.RoomAmenities.Where(a => a.RoomID == room.ID).ToListAsync();
            }

            return await rooms.ToListAsync();
        }

        /// <summary>
        /// Updates an existing Room entry with data from the Edit view form, verified by the controller
        /// </summary>
        /// <param name="room">Edited room data from form</param>
        /// <returns>List of rooms with updated entry</returns>
        public async Task UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a selected Room entry from the table, then deletes any HotelRoom table entries with the same room name, and finally deletes any RoomAmenities entries with the same room name.
        /// </summary>
        /// <param name="id">Selected room ID</param>
        /// <returns>List of rooms minus the deleted one</returns>
        public async Task DeleteRoom(int id)
        {
            Room room = _context.Rooms.FirstOrDefault(r => r.ID == id);
            _context.Rooms.Remove(room);

            List<HotelRoom> roomTemplate = _context.HotelRooms.Where(r => r.Room.Name == room.Name).ToList();

            foreach (HotelRoom template in roomTemplate)
            {
                _context.HotelRooms.Remove(template);
            }

            List<RoomAmenities> roomAmens = _context.RoomAmenities.Where(a => a.Rooms.Name == room.Name).ToList();

            foreach (RoomAmenities template in roomAmens)
            {
                _context.RoomAmenities.Remove(template);
            }

            await _context.SaveChangesAsync();
        }
    }
}
