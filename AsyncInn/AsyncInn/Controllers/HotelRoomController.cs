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
        public async Task<IActionResult> Index()
        {
            var asyncInnDbContext = _context.HotelRooms.Include(h => h.Hotel).Include(r =>  r.Room);
            return View(await asyncInnDbContext.ToListAsync());
        }

        // GET: HotelRoom/Details/5
        public async Task<IActionResult> Details(int? roomId, int? hotelId)
        {
            if (hotelId == null || roomId == null)
            {
                return NotFound();
            }

            var hotelRoom = await _context.HotelRooms.Include(h => h.Hotel)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(hr => hr.RoomID == roomId && hr.HotelID == hotelId);
            if (hotelRoom == null)
            {
                return NotFound();
            }

            return View(hotelRoom);
        }

        // GET: HotelRoom/Create
        public IActionResult Create()
        {
            ViewData["HotelID"] = new SelectList(_context.Hotels, "ID", "Name");
            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name");
            return View();
        }

        // POST: HotelRoom/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelID,RoomNumberID,RoomID,Rate,PetFriendly")] HotelRoom hotelRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotelRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoomID"] = new SelectList(_context.Rooms, "ID", "Name", hotelRoom.RoomID);
            ViewData["HotelID"] = new SelectList(_context.Hotels, "ID", "Name", hotelRoom.HotelID);
            return View(hotelRoom);
        }

        // GET: HotelRoom/Edit/5
        public async Task<IActionResult> Edit(int? roomId, int? hotelId)
        {
            if (roomId == null || hotelId == null)
            {
                return NotFound();
            }

            var hotelRoom = await _context.HotelRooms.Include(h => h.Hotel)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(hr => hr.RoomID == roomId && hr.HotelID == hotelId);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int roomId, int hotelId, [Bind("HotelID,RoomNumberID,RoomID,Rate,PetFriendly")] HotelRoom hotelRoom)
        {
            if (hotelId != hotelRoom.HotelID || roomId != hotelRoom.RoomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotelRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelRoomExists(hotelRoom.RoomID, hotelRoom.HotelID))
                    {
                        return NotFound();
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
        public async Task<IActionResult> Delete(int? roomId, int? hotelId)
        {
            if (roomId == null || hotelId == null)
            {
                return NotFound();
            }

            var hotelRoom = await _context.HotelRooms.Include(h => h.Hotel)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(hr => hr.RoomID == roomId && hr.HotelID == hotelId);
            if (hotelRoom == null)
            {
                return NotFound();
            }

            return View(hotelRoom);
        }

        // POST: HotelRoom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int roomId, int hotelId)
        {
            var hotelRoom = await _context.HotelRooms.Include(h => h.Hotel)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(hr => hr.RoomID == roomId && hr.HotelID == hotelId);
            _context.HotelRooms.Remove(hotelRoom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelRoomExists(int roomId, int hotelId)
        {
            return _context.HotelRooms.Any(e => e.HotelID == hotelId && e.RoomID == roomId);
        }
    }
}
