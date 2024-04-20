using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DaNangTourism.Data;
using DaNangTourism.Models;

namespace DaNangTourism.Controllers
{
    public class ScheduleDestinationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScheduleDestinationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScheduleDestinations
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScheduleDestination.ToListAsync());
        }

        // GET: ScheduleDestinations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleDestination = await _context.ScheduleDestination
                .FirstOrDefaultAsync(m => m.SDID == id);
            if (scheduleDestination == null)
            {
                return NotFound();
            }

            return View(scheduleDestination);
        }

        // GET: ScheduleDestinations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScheduleDestinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SDID,destinationID,scheduleID,arrivalTime,leaveTime,costEstimate,note")] ScheduleDestination scheduleDestination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduleDestination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scheduleDestination);
        }

        // GET: ScheduleDestinations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleDestination = await _context.ScheduleDestination.FindAsync(id);
            if (scheduleDestination == null)
            {
                return NotFound();
            }
            return View(scheduleDestination);
        }

        // POST: ScheduleDestinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SDID,destinationID,scheduleID,arrivalTime,leaveTime,costEstimate,note")] ScheduleDestination scheduleDestination)
        {
            if (id != scheduleDestination.SDID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scheduleDestination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleDestinationExists(scheduleDestination.SDID))
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
            return View(scheduleDestination);
        }

        // GET: ScheduleDestinations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scheduleDestination = await _context.ScheduleDestination
                .FirstOrDefaultAsync(m => m.SDID == id);
            if (scheduleDestination == null)
            {
                return NotFound();
            }

            return View(scheduleDestination);
        }

        // POST: ScheduleDestinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var scheduleDestination = await _context.ScheduleDestination.FindAsync(id);
            if (scheduleDestination != null)
            {
                _context.ScheduleDestination.Remove(scheduleDestination);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleDestinationExists(string id)
        {
            return _context.ScheduleDestination.Any(e => e.SDID == id);
        }
    }
}
