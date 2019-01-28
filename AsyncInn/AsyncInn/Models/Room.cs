using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Layouts Layout { get; set; }

        // Navigation
        public ICollection<HotelRoom> HotelRooms { get; set; }
        public ICollection<RoomAmenities> RoomID { get; set; }
    }

    public enum Layouts
    {
        Studio,
        OneBedroom,
        TwoBedroom
    }
}
