using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models
{
    public class HotelRoom
    {
        public int HotelID { get; set; }
        public int RoomNumberID { get; set; }
        [Required]
        public int RoomID { get; set; }
        [Required]
        public decimal Rate { get; set; }
        public bool PetFriendly { get; set; }

        // Navigation
        public Hotel Hotel { get; set; }
        public Room Room { get; set; }
    }
}
