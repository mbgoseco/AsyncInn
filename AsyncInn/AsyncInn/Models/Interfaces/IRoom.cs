using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IRoom
    {
        // Create Room
        Task CreateRoom(Room Room);

        // Read Room and return specifc ones
        Task<Room> GetRoom(int id);

        // Read all rooms
        Task<IEnumerable<Room>> GetRooms(string searchRooms);

        // Update Room
        Task UpdateRoom(Room Room);

        // Delete Room
        Task DeleteRoom(int id);
    }
}
