#nullable disable
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
    public class AddDVDController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddDVDController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AddDVD
        public async Task<IActionResult> Index()
        {
            return View(await _context.DVDTitles.ToListAsync());
        }

        // GET: AddDVD/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDTitle = await _context.DVDTitles
                .FirstOrDefaultAsync(m => m.DVDNumber == id);
            if (dVDTitle == null)
            {
                return NotFound();
            }

            return View(dVDTitle);
        }

        // GET: AddDVD/Create
        public IActionResult Create()
        {
            ViewBag.producer =_context.Producers.ToArray();
            ViewBag.category = _context.DVDCategories.ToArray();
            ViewBag.studio = _context.Studios.ToArray();
            return View();
        }

        // POST: AddDVD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DVDNumber,CategoryNumber,StudioNumber,ProducerNumber,DVDTitles,DateReleased,StandardCharge,PenaltyCharge")] DVDTitle dVDTitle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dVDTitle);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "CastMembers");
            }
            return View(dVDTitle);
        }

        // GET: AddDVD/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDTitle = await _context.DVDTitles.FindAsync(id);
            if (dVDTitle == null)
            {
                return NotFound();
            }
            return View(dVDTitle);
        }

        // POST: AddDVD/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DVDNumber,CategoryNumber,StudioNumber,ProducerNumber,DVDTitles,DateReleased,StandardCharge,PenaltyCharge")] DVDTitle dVDTitle)
        {
            if (id != dVDTitle.DVDNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dVDTitle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DVDTitleExists(dVDTitle.DVDNumber))
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
            return View(dVDTitle);
        }

        // GET: AddDVD/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dVDTitle = await _context.DVDTitles
                .FirstOrDefaultAsync(m => m.DVDNumber == id);
            if (dVDTitle == null)
            {
                return NotFound();
            }

            return View(dVDTitle);
        }

        // POST: AddDVD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dVDTitle = await _context.DVDTitles.FindAsync(id);
            _context.DVDTitles.Remove(dVDTitle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DVDTitleExists(int id)
        {
            return _context.DVDTitles.Any(e => e.DVDNumber == id);
        }
    }
}
