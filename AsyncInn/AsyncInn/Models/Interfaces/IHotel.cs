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

        // Read Hotel and return specific ones
        Task<Hotel> GetHotel(int id);

        // Read all hotels
        Task<IEnumerable<Hotel>> GetHotels(string searchHotels);

        // Update Hotel
        Task UpdateHotel(Hotel hotel);

        // Delete Hotel
        Task DeleteHotel(int id);
    }
}
