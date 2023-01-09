using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatabaseCoursework.Models;
using groupCW.Data;

namespace groupCW.Controllers
{
    public class DVDCopiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DVDCopiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DVDCopies
        public async Task<IActionResult> Index()
        {
              return _context.DVDCopies != null ? 
                          View(await _context.DVDCopies.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DVDCopies'  is null.");
        }

        // GET: DVDCopies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DVDCopies == null)
            {
                return NotFound();
            }

            var dVDCopy = await _context.DVDCopies
                .FirstOrDefaultAsync(m => m.CopyNumber == id);
            if (dVDCopy == null)
            {
                return NotFound();
            }

            return View(dVDCopy);
        }

        // GET: DVDCopies/Create
        public IActionResult Create()
        {
            ViewBag.DVD = _context.DVDTitles.ToArray();
            return View();
        }

        // POST: DVDCopies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CopyNumber,DVDNumber,DatePurchased")] DVDCopy dVDCopy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dVDCopy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dVDCopy);
        }

        // GET: DVDCopies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DVDCopies == null)
            {
                return NotFound();
            }

            var dVDCopy = await _context.DVDCopies.FindAsync(id);
            if (dVDCopy == null)
            {
                return NotFound();
            }
            return View(dVDCopy);
        }

        // POST: DVDCopies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CopyNumber,DVDNumber,DatePurchased")] DVDCopy dVDCopy)
        {
            if (id != dVDCopy.CopyNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dVDCopy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DVDCopyExists(dVDCopy.CopyNumber))
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
            return View(dVDCopy);
        }

        // GET: DVDCopies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DVDCopies == null)
            {
                return NotFound();
            }

            var dVDCopy = await _context.DVDCopies
                .FirstOrDefaultAsync(m => m.CopyNumber == id);
            if (dVDCopy == null)
            {
                return NotFound();
            }

            return View(dVDCopy);
        }

        // POST: DVDCopies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DVDCopies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DVDCopies'  is null.");
            }
            var dVDCopy = await _context.DVDCopies.FindAsync(id);
            if (dVDCopy != null)
            {
                _context.DVDCopies.Remove(dVDCopy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DVDCopyExists(int id)
        {
          return (_context.DVDCopies?.Any(e => e.CopyNumber == id)).GetValueOrDefault();
        }
    }
}
