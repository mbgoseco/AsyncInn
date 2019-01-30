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

        // Read Room
        Task<Room> GetRoom(int id);

        Task<IEnumerable<Room>> GetRooms();

        // Update Room
        Task UpdateRoom(Room Room);

        // Delete Room
        Task DeleteRoom(int id);
    }
}
