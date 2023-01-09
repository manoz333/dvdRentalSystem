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
    public class MembershipCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MembershipCategories
        public async Task<IActionResult> Index()
        {
              return _context.MembershipCategories != null ? 
                          View(await _context.MembershipCategories.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.MembershipCategories'  is null.");
        }

        // GET: MembershipCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MembershipCategories == null)
            {
                return NotFound();
            }

            var membershipCategory = await _context.MembershipCategories
                .FirstOrDefaultAsync(m => m.MembershipCatagoryNumber == id);
            if (membershipCategory == null)
            {
                return NotFound();
            }

            return View(membershipCategory);
        }

        // GET: MembershipCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MembershipCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipCatagoryNumber,MembershipCategoryDescription,MembershipCategoryTotalLoans")] MembershipCategory membershipCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membershipCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(membershipCategory);
        }

        // GET: MembershipCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MembershipCategories == null)
            {
                return NotFound();
            }

            var membershipCategory = await _context.MembershipCategories.FindAsync(id);
            if (membershipCategory == null)
            {
                return NotFound();
            }
            return View(membershipCategory);
        }

        // POST: MembershipCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembershipCatagoryNumber,MembershipCategoryDescription,MembershipCategoryTotalLoans")] MembershipCategory membershipCategory)
        {
            if (id != membershipCategory.MembershipCatagoryNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membershipCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipCategoryExists(membershipCategory.MembershipCatagoryNumber))
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
            return View(membershipCategory);
        }

        // GET: MembershipCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MembershipCategories == null)
            {
                return NotFound();
            }

            var membershipCategory = await _context.MembershipCategories
                .FirstOrDefaultAsync(m => m.MembershipCatagoryNumber == id);
            if (membershipCategory == null)
            {
                return NotFound();
            }

            return View(membershipCategory);
        }

        // POST: MembershipCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MembershipCategories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MembershipCategories'  is null.");
            }
            var membershipCategory = await _context.MembershipCategories.FindAsync(id);
            if (membershipCategory != null)
            {
                _context.MembershipCategories.Remove(membershipCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipCategoryExists(int id)
        {
          return (_context.MembershipCategories?.Any(e => e.MembershipCatagoryNumber == id)).GetValueOrDefault();
        }
    }
}
