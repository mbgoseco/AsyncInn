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

namespace AsyncInn.Controllers
{
    public class AmenitiesController : Controller
    {
        private readonly IAmenities _context;

        public AmenitiesController(IAmenities context)
        {
            _context = context;
        }

        // GET: Amenities
        /// <summary>
        /// Calls the GetAmenites service to get a list of all amenities or a search filtered amenity and display the results to the Index view.
        /// </summary>
        /// <param name="searchAmenities">Optional search parameter</param>
        /// <returns>Index view with resulting list of amenities</returns>
        public async Task<IActionResult> Index(string searchAmenities)
        {
            return View(await _context.GetAmenities(searchAmenities));
        }

        // GET: Amenities/Details/5
        /// <summary>
        /// Calls the GetAmenites service to return the details of a specific amenity ID, if it exists, and display it in the Details view.
        /// </summary>
        /// <param name="id">Primary Key value</param>
        /// <returns>Details view with selected amenity</returns>
        public async Task<IActionResult> Details(int id)
        {
            var amenities = await _context.GetAmenities(id);
            if (amenities == null)
            {
                return NotFound();
            }

            return View(amenities);
        }

        // GET: Amenities/Create
        /// <summary>
        /// Takes user to the Create view to fill out a form for a new amenity.
        /// </summary>
        /// <returns>Create amenity view</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Amenities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Calls the CreateAmenities service to add a new amenity to the table based on the form data, then returns to the Index view.
        /// </summary>
        /// <param name="amenities">New Amenities object</param>
        /// <returns>Index view including new amenity</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Amenities amenities)
        {
            if (ModelState.IsValid)
            {
                await _context.CreateAmenities(amenities);
                return RedirectToAction(nameof(Index));
            }
            return View(amenities);
        }

        // GET: Amenities/Edit/5
        /// <summary>
        /// Calls the GetAmenities service to select a specific amenity, if it exists, and bring it to the Edit view for editing.
        /// </summary>
        /// <param name="id">Primary Key value of selected amenity</param>
        /// <returns>Edit view or Not Found</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var amenities = await _context.GetAmenities(id);
            if (amenities == null)
            {
                return NotFound();
            }
            return View(amenities);
        }

        // POST: Amenities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Takes in a new amenities object from the edit form and assigns its properties to the chosen amenity from the table, providing the id matches and the values of the properties are valid.
        /// </summary>
        /// <param name="id">Primary Key value of chosen amenity</param>
        /// <param name="amenities">New amenities object with edit data</param>
        /// <returns>Index view with edited entry</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Amenities amenities)
        {
            if (id != amenities.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateAmenities(amenities);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AmenitiesExists(amenities.ID))
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
            return View(amenities);
        }

        // GET: Amenities/Delete/5
        /// <summary>
        /// Takes user to the Delete view with a selected amenity, asking the user if they want to delete it from the table.
        /// </summary>
        /// <param name="id">Primary Key of selected amenity</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            var amenities = await _context.GetAmenities(id);
            if (amenities == null)
            {
                return NotFound();
            }

            return View(amenities);
        }

        // POST: Amenities/Delete/5
        /// <summary>
        /// Calls the DeleteAmenities service to delete the chosen amenity from the table, if it exists, and return to the Index view.
        /// </summary>
        /// <param name="id">Primary Key value of chosen amenity</param>
        /// <returns>Index view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _context.DeleteAmenities(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AmenitiesExists(int id)
        {
            return _context.GetAmenities(id).Equals(id);
        }
    }
}
