using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models;

namespace AsyncInn.Controllers
{
    public class RoomAmenitiesController : Controller
    {
        private readonly AsyncInnDbContext _context;

        public RoomAmenitiesController(AsyncInnDbContext context)
        {
            _context = context;
        }

        // GET: RoomAmenities
        /// <summary>
        /// Gets all entries in the RoomAmenities table and displays them to the Index view.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var asyncInnDbContext = _context.RoomAmenities.Include(r => r.Amenities).Include(r => r.Rooms);
            return View(await asyncInnDbContext.ToListAsync());
        }

        // GET: RoomAmenities/Create
        /// <summary>
        /// Displays the view allowing users to make a new RoomAmenites entry
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewData["AmenitiesID"] = new SelectList(_context.Amenities, "ID", "Name");
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name");
            return View();
        }

        // POST: RoomAmenities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Sends the from information from the view form to the server and adds it to the RoomAmenities table
        /// </summary>
        /// <param name="roomAmenities">New RoomAmenities properties from the form</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AmenitiesID,RoomID")] RoomAmenities roomAmenities)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomAmenities);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AmenitiesID"] = new SelectList(_context.Amenities, "ID", "Name", roomAmenities.AmenitiesID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name", roomAmenities.RoomID);
            return View(roomAmenities);
        }

        // GET: RoomAmenities/Delete/5
        /// <summary>
        /// Displays a chosen RoomAmenities entry and prompts the user to decide to delete it from the table
        /// </summary>
        /// <param name="amenitiesId">Chosen amenity name</param>
        /// <param name="roomId">Chosen room name</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? amenitiesId, int? roomId)
        {
            if (amenitiesId == null || roomId == null)
            {
                return NotFound();
            }

            var roomAmenities = await _context.RoomAmenities
                .Include(r => r.Amenities)
                .Include(r => r.Rooms)
                .FirstOrDefaultAsync(m => m.AmenitiesID == amenitiesId && m.RoomID == roomId);
            if (roomAmenities == null)
            {
                return NotFound();
            }

            return View(roomAmenities);
        }

        // POST: RoomAmenities/Delete/5
        /// <summary>
        /// Deletes the selected RoomAmenities entry from the table
        /// </summary>
        /// <param name="amenitiesId">Chosen amenity name</param>
        /// <param name="roomId">Chosen room name</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? amenitiesId, int? roomId)
        {
            var roomAmenities = await _context.RoomAmenities
                .FirstOrDefaultAsync(m => m.AmenitiesID == amenitiesId && m.RoomID == roomId);
            _context.RoomAmenities.Remove(roomAmenities);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomAmenitiesExists(int amenitiesId, int roomId)
        {
            return _context.RoomAmenities.Any(e => e.AmenitiesID == amenitiesId && e.RoomID == roomId);
        }
    }
}
