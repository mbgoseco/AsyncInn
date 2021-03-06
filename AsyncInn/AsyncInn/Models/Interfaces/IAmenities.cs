﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncInn.Models.Interfaces
{
    public interface IAmenities
    {
        // Create Amenities
        Task CreateAmenities(Amenities Amenities);

        // Read Amenities and return specific ones
        Task<Amenities> GetAmenities(int id);

        // Read all amenities
        Task<IEnumerable<Amenities>> GetAmenities(string searchAmenities);

        // Update Amenities
        Task UpdateAmenities(Amenities Amenities);

        // Delete Amenities
        Task DeleteAmenities(int id);
    }
}
