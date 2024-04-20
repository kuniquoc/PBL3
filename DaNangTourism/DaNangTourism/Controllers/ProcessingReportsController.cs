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
    public class ProcessingReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessingReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessingReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProcessingReport.ToListAsync());
        }

        // GET: ProcessingReports/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processingReport = await _context.ProcessingReport
                .FirstOrDefaultAsync(m => m.prcReportID == id);
            if (processingReport == null)
            {
                return NotFound();
            }

            return View(processingReport);
        }

        // GET: ProcessingReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProcessingReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("prcReportID,prcContent,processTime")] ProcessingReport processingReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(processingReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processingReport);
        }

        // GET: ProcessingReports/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processingReport = await _context.ProcessingReport.FindAsync(id);
            if (processingReport == null)
            {
                return NotFound();
            }
            return View(processingReport);
        }

        // POST: ProcessingReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("prcReportID,prcContent,processTime")] ProcessingReport processingReport)
        {
            if (id != processingReport.prcReportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processingReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessingReportExists(processingReport.prcReportID))
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
            return View(processingReport);
        }

        // GET: ProcessingReports/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processingReport = await _context.ProcessingReport
                .FirstOrDefaultAsync(m => m.prcReportID == id);
            if (processingReport == null)
            {
                return NotFound();
            }

            return View(processingReport);
        }

        // POST: ProcessingReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var processingReport = await _context.ProcessingReport.FindAsync(id);
            if (processingReport != null)
            {
                _context.ProcessingReport.Remove(processingReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessingReportExists(string id)
        {
            return _context.ProcessingReport.Any(e => e.prcReportID == id);
        }
    }
}
