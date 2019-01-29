using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IHotel
    {
        // Create Hotel
        Task CreateHotel(Hotel hotel);

        // Read Hotel
        Task<Hotel> GetHotel(int id);

        Task<IEnumerable<Hotel>> GetHotels();

        // Update Hotel
        Task UpdateHotel(Hotel hotel);

        // Delete Hotel
        Task DeleteHotel(int id);
    }
}
