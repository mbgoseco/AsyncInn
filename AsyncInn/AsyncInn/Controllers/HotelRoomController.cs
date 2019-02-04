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
    public class HotelRoomController : Controller
    {
        private readonly AsyncInnDbContext _context;

        public HotelRoomController(AsyncInnDbContext context)
        {
            _context = context;
        }

        // GET: HotelRoom
        /// <summary>
        /// Gets all entries in the HotelRooms table and displays them to the Index view.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var asyncInnDbContext = _context.HotelRooms.Include(h => h.Hotel).Include(r =>  r.Room);
            return View(await asyncInnDbContext.ToListAsync());
        }

        // GET: HotelRoom/Details/5
        /// <summary>
        /// Displays a single selected row in the HotelRooms table
        /// </summary>
        /// <param name="roomNumberId">Room number</param>
        /// <param name="hotelId">Hotel name</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? roomNumberId, int? hotelId)
        {
            if (hotelId == null || roomNumberId == null)
            {
                return NotFound();
            }

            var hotelRoom = await _context.HotelRooms.Include(h => h.Hotel)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(hr => hr.RoomNumberID == roomNumberId && hr.HotelID == hotelId);
            if (hotelRoom == null)
            {
                return NotFound();
            }

            return View(hotelRoom);
        }

        // GET: HotelRoom/Create
        /// <summary>
        /// Displays the view allowing a user to enter a new HotelRoom
        /// </summary>
        /// <param name="isDuplicate">Conditional param returning a message if the user entered a duplicate room number and hotel combo.</param>
        /// <returns></returns>
        public IActionResult Create(bool isDuplicate)
        {
            if (isDuplicate)
            {
                ViewData["errorMsg"] = "This room number already exists at this hotel. Please enter another.";
            }
            else
            {
                ViewData["errorMsg"] = "";
            }
            ViewData["HotelID"] = new SelectList(_context.Hotels, "ID", "Name");
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name");
            return View();
        }

        // POST: HotelRoom/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Sends the from information from the view to the server and checks if the hotel and room are duplicates, then passes it to EF to add to the HotelRoom table.
        /// </summary>
        /// <param name="hotelRoom">HotelRoom properties from the form</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelID,RoomNumberID,RoomID,Rate,PetFriendly")] HotelRoom hotelRoom)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = false;

                if (HotelRoomExists(hotelRoom.RoomNumberID, hotelRoom.HotelID))
                {
                    isDuplicate = true;
                    return RedirectToAction("Create", new { isDuplicate });
                }
                else
                { 
                ViewData["errorMsg"] = "";
                _context.Add(hotelRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
            }

            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name", hotelRoom.RoomID);
            ViewData["HotelID"] = new SelectList(_context.Hotels, "ID", "Name", hotelRoom.HotelID);
            return View(hotelRoom);
        }

        // GET: HotelRoom/Edit/5
        /// <summary>
        /// Displays a selected HotelRoom entry for editing
        /// </summary>
        /// <param name="roomNumberId">Room number</param>
        /// <param name="hotelId">Hotel name</param>
        /// <param name="isDuplicate">Conditional param returning a message if the user entered a duplicate room number and hotel combo.</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? roomNumberId, int? hotelId, bool isDuplicate)
        {
            if (isDuplicate)
            {
                ViewData["errorMsg"] = "This room number already exists at this hotel. Please enter another.";
            }
            else
            {
                ViewData["errorMsg"] = "";
            }

            if (roomNumberId == null || hotelId == null)
            {
                return NotFound();
            }

            var hotelRoom = await _context.HotelRooms
                .FirstOrDefaultAsync(hr => hr.RoomNumberID == roomNumberId && hr.HotelID == hotelId);
            if (hotelRoom == null)
            {
                return NotFound();
            }
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name", hotelRoom.RoomID);
            ViewData["HotelID"] = new SelectList(_context.Hotels, "ID", "Name", hotelRoom.HotelID);
            return View(hotelRoom);
        }

        // POST: HotelRoom/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Sends the from information from the view to the server and checks if the hotel and room are duplicates, then passes it to EF to update the selected HotelRoom entry.
        /// </summary>
        /// <param name="roomNumberId">Room number</param>
        /// <param name="hotelId">Hotel name</param>
        /// <param name="hotelRoom">Edited hotel room properties from the form</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int roomNumberId, int hotelId, [Bind("HotelID,RoomNumberID,RoomID,Rate,PetFriendly")] HotelRoom hotelRoom)
        {

            if (ModelState.IsValid)
            {
                bool isDuplicate = false;

                try
                {
                    _context.Update(hotelRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (HotelRoomExists(hotelRoom.RoomNumberID, hotelRoom.HotelID))
                    {
                        isDuplicate = true;
                        return RedirectToAction("Edit", new { isDuplicate });
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name", hotelRoom.RoomID);
            ViewData["HotelID"] = new SelectList(_context.Hotels, "ID", "Name", hotelRoom.HotelID);
            return View(hotelRoom);
        }

        // GET: HotelRoom/Delete/5
        /// <summary>
        /// Displays a chosen HotelRoom entry and prompts the user to decide to delete it from the table
        /// </summary>
        /// <param name="roomNumberId">Selected room number</param>
        /// <param name="hotelId">Selected hotel name</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? roomNumberId, int? hotelId)
        {
            if (roomNumberId == null || hotelId == null)
            {
                return NotFound();
            }

            var hotelRoom = await _context.HotelRooms.Include(h => h.Hotel)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(hr => hr.RoomNumberID == roomNumberId && hr.HotelID == hotelId);
            if (hotelRoom == null)
            {
                return NotFound();
            }

            return View(hotelRoom);
        }

        // POST: HotelRoom/Delete/5
        /// <summary>
        /// Deletes the selected HotelRoom entry from the table
        /// </summary>
        /// <param name="roomNumberId">Selected room number</param>
        /// <param name="hotelId">Selected hotel name</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int roomNumberId, int hotelId)
        {
            var hotelRoom = await _context.HotelRooms
                .FirstOrDefaultAsync(hr => hr.RoomNumberID == roomNumberId && hr.HotelID == hotelId);
            _context.HotelRooms.Remove(hotelRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelRoomExists(int roomNumberId, int hotelId)
        {
            return _context.HotelRooms.Any(e => e.HotelID == hotelId && e.RoomNumberID == roomNumberId);
        }
    }
}
