using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models
{
    public class Room
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public Layouts Layout { get; set; }

        // Navigation
        public ICollection<HotelRoom> Hotels { get; set; }
        public ICollection<RoomAmenities> RoomID { get; set; }
    }

    public enum Layouts
    {
        Studio,
        [Display(Name="1 Bedroom")]
        OneBedroom,
        [Display(Name="2 Bedroom")]
        TwoBedroom
    }
}
