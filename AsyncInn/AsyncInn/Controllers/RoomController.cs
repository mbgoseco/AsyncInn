﻿using System;
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
    public class RoomController : Controller
    {
        private readonly IRoom _context;

        public RoomController(IRoom context)
        {
            _context = context;
        }

        // GET: Room
        /// <summary>
        /// Calls the GetRooms service to get a list of all rooms or a search filtered room and display the results to the Index view.
        /// </summary>
        /// <param name="searchRooms">Optional search parameter</param>
        /// <returns>Index view with resulting list of rooms</returns>
        public async Task<IActionResult> Index(string searchRooms)
        {
            return View(await _context.GetRooms(searchRooms));
        }

        // GET: Room/Details/5
        /// <summary>
        /// Calls the GetRooms service to return the details of a specific room ID, if it exists, and display it in the Details view.
        /// </summary>
        /// <param name="id">Primary Key value</param>
        /// <returns>Details view with selected room</returns>
        public async Task<IActionResult> Details(int id)
        {
            var room = await _context.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Room/Create
        /// <summary>
        /// Takes user to the Create view to fill out a form for a new room.
        /// </summary>
        /// <returns>Create room view</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Room/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Calls the CreateRoom service to add a new room to the table based on the form data, then returns to the Index view.
        /// </summary>
        /// <param name="room">New room object</param>
        /// <returns>Index view including new room</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Layout")] Room room)
        {
            if (ModelState.IsValid)
            {
                await _context.CreateRoom(room);
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Room/Edit/5
        /// <summary>
        /// Calls the GetRoom service to select a specific room, if it exists, and bring it to the Edit view for editing.
        /// </summary>
        /// <param name="id">Primary Key value of selected room</param>
        /// <returns>Edit view or Not Found</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Takes in a new Rooom object from the edit form and assigns its properties to the chosen room from the table, providing the id matches and the values of the properties are valid.
        /// </summary>
        /// <param name="id">Primary Key value of chosen room</param>
        /// <param name="room">New Room object with edit data</param>
        /// <returns>Index view with edited entry</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Layout")] Room room)
        {
            if (id != room.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.UpdateRoom(room);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.ID))
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
            return View(room);
        }

        // GET: Room/Delete/5
        /// <summary>
        /// Takes user to the Delete view with a selected room, asking the user if they want to delete it from the table.
        /// </summary>
        /// <param name="id">Primary Key of selected room</param>
        /// <returns>Delete view of room selected for deletion</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Room/Delete/5
        /// <summary>
        /// Calls the DeleteRoom service to delete the chosen room from the table, if it exists, and return to the Index view.
        /// </summary>
        /// <param name="id">Primary Key value of chosen room</param>
        /// <returns>Index view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _context.DeleteRoom(id);
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.GetRoom(id).Equals(id);
        }
    }
}
