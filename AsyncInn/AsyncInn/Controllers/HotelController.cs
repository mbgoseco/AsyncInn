using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsyncInn.Data;
using AsyncInn.Models;
using AsyncInn.Models.Interfaces;
using AsyncInn.Models.Services;


namespace AsyncInn.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotel _context;

        public HotelController(IHotel context)
        {
            _context = context;
        }

        // GET: Hotel
        /// <summary>
        /// Calls the GetHotels service to get a list of all hotels or a search filtered hotel and display the results to the Index view.
        /// </summary>
        /// <param name="searchHotels">Optional search parameter</param>
        /// <returns>Index view with resulting list of hotels</returns>
        public async Task<IActionResult> Index(string searchHotels)
        {
            return View(await _context.GetHotels(searchHotels));
        }

        // GET: Hotel/Details/5
        /// <summary>
        /// Calls the GetHotel service to return the details of a specific hotel ID, if it exists, and display it in the Details view.
        /// </summary>
        /// <param name="id">Primary Key value</param>
        /// <returns>Details view with selected hotel</returns>
        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _context.GetHotel(id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotel/Create
        /// <summary>
        /// Takes user to the Create view to fill out a form for a new hotel.
        /// </summary>
        /// <returns>Create hotel view</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Calls the CreateHotel service to add a new hotel to the table based on the form data, then returns to the Index view.
        /// </summary>
        /// <param name="hotel">New Hotel object</param>
        /// <returns>Index view including new hotel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Address,Phone")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                await _context.CreateHotel(hotel);
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotel/Edit/5
        /// <summary>
        /// Calls the GetHotel service to select a specific hotel, if it exists, and bring it to the Edit view for editing.
        /// </summary>
        /// <param name="id">Primary Key value of selected hotel</param>
        /// <returns>Edit view or Not Found</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await _context.GetHotel(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Takes in a new Hotel object from the edit form and assigns its properties to the chosen hotel from the table, providing the id matches and the values of the properties are valid.
        /// </summary>
        /// <param name="id">Primary Key value of chosen hotel</param>
        /// <param name="hotel">New hotel object with edit data</param>
        /// <returns>Index view with edited entry</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Address,Phone")] Hotel hotel)
        {
            if (id != hotel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateHotel(hotel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.ID))
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
            return View(hotel);
        }

        // GET: Hotel/Delete/5
        /// <summary>
        /// Takes user to the Delete view with a selected hotel, asking the user if they want to delete it from the table.
        /// </summary>
        /// <param name="id">Primary Key of selected hotel</param>
        /// <returns>Delete view of hotel selected for deletion</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _context.GetHotel(id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotel/Delete/5
        /// <summary>
        /// Calls the DeleteHotel service to delete the chosen hotel from the table, if it exists, and return to the Index view.
        /// </summary>
        /// <param name="id">Primary Key value of chosen hotel</param>
        /// <returns>Index view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _context.DeleteHotel(id);
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return _context.GetHotel(id).Equals(id);
        }
    }
}
