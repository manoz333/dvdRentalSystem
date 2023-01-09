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
using groupCW.ViewModel;

namespace groupCW.Controllers
{
    public class AddLoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddLoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AddLoans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Loans.Include(l => l.DVDCopy).Include(l => l.LoanType).Include(l => l.Member);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AddLoans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.DVDCopy)
                .Include(l => l.LoanType)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LoanNumber == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: AddLoans/Create
        public IActionResult Create()
        {
            //ViewData["CopyNumber"] = new SelectList(_context.DVDCopies, "CopyNumber", "CopyNumber");
            //ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanTypeNumber");
            //ViewData["MemberNumber"] = new SelectList(_context.Members, "MemberNumber", "MemberNumber");

            IEnumerable<RestrictionDuringLoan> loan = _context.Loans.Join(_context.DVDCopies,
                loan => loan.CopyNumber, copy => copy.CopyNumber,
                (loan, copy) => new RestrictionDuringLoan()
                {
                    dateReturned = loan.DateReturned,
                    copyNumber = copy.CopyNumber
                }
            ).Where(x => x.dateReturned == null).ToList();

            var dvdCopies = _context.DVDCopies.ToList();

            List<int> itemCopy = new List<int>();
            foreach (var item in dvdCopies)
            {
                itemCopy.Add(item.CopyNumber);

            }

            List<int> copyNumberss = new List<int>();

            foreach (var loans in loan)
            {
                copyNumberss.Add(loans.copyNumber);

            }
            var haha = itemCopy.Except(copyNumberss).ToList();
            ViewBag.loantype = _context.LoanTypes.ToArray();
            ViewBag.copynumber = haha.ToArray();
            ViewBag.membernumber = _context.Members.ToArray();
            return View();
        }

        // POST: AddLoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanNumber,LoanTypeNumber,CopyNumber,MemberNumber,DateOut,DateDue,DateReturned")] Loan loan)
        {
            int membernumber = loan.MemberNumber;
            //return Json(memberNumber);
            int loanNumberrrr = loan.LoanTypeNumber;

            int copyNumber = loan.CopyNumber;

            List<RestrictionDuringLoan> loanedOut = _context.Members.Join(
                _context.MembershipCategories,
                membershipcat => membershipcat.MembershipCategoryNumber,
                member => member.MembershipCatagoryNumber,
                (membershipcat, member) => new RestrictionDuringLoan()
                {
                    memberNumber = membershipcat.MemberNumber,
                    memberfName = membershipcat.MemberFirstName,
                    memberlName = membershipcat.MemberLastName,
                    membershipDesc = member.MembershipCategoryDescription,
                    memberDateOfBirth = membershipcat.MemberDateOfBirth,
                    //membershipTotalLoan = member.MembershipCategoryTotalLoans,
                }).Where(x => x.memberNumber == membernumber).ToList();



            //get the age restricted or not categories 
            List<RestrictionDuringLoan> loanedOut2 = _context.DVDCopies.Join(_context.DVDTitles,
                    copies => copies.DVDNumber, dvdtitle => dvdtitle.DVDNumber,
                    (copies, dvdtitle) => new
                    {
                        copyNumber = copies.CopyNumber,
                        dvdcat = dvdtitle.CategoryNumber
                    })
                .Join(_context.DVDCategories,
                    dvdtitle => dvdtitle.dvdcat, dvdcate => dvdcate.CategoryNumber,
                    (dvdtitle, dvdcate) => new RestrictionDuringLoan()
                    {
                        copyNumber = dvdtitle.copyNumber,
                        ageRestricted = dvdcate.AgeRestricted
                    }).Where(x => x.copyNumber == copyNumber).ToList();




            //get the loan type and duration 
            List<RestrictionDuringLoan> loanedOut3 = _context.Loans.Join(_context.LoanTypes,
                loan => loan.LoanTypeNumber, loantype => loantype.LoanTypeNumber,
                (loan, loantype) => new RestrictionDuringLoan()
                {
                    memberNumber = loan.MemberNumber,
                    copyNumber = loan.CopyNumber,
                    loanduration = loantype.LoanTypeNumber,
                    loantype = loantype.LoanDuration
                }
            ).Where(x => x.loanduration == loanNumberrrr).ToList();

            foreach (var item in loanedOut)
            {
                var checkAge = Convert.ToDateTime(item.memberDateOfBirth);
                int age = 0;
                age = DateTime.Now.Subtract(checkAge).Days;
                age = age / 365;
                foreach (var items in loanedOut2)
                {
                    if (age < 18 && items.ageRestricted == "yes")
                    {
                        return Json("This person is age restricted to buy the DVD");
                    }
                }

            }

            //check total loans 

            List<TotalDVDLoanViewModel> t2 = _context.MembershipCategories.Join(_context.Members,
                   mbscategory => mbscategory.MembershipCatagoryNumber,
                   members => members.MembershipCategoryNumber,
                   (mbscategory, members) => new TotalDVDLoanViewModel
                   {
                       MemberNumber = members.MemberNumber,
                       FirstName = members.MemberFirstName,
                       LastName = members.MemberLastName,
                       Address = members.MemberAddress,
                       DateOfBirth = members.MemberDateOfBirth,
                       MembershipCategoryDescription = mbscategory.MembershipCategoryDescription,
                       MembershipCategoryTotalLoans = (mbscategory.MembershipCategoryTotalLoans == null ? 0 : mbscategory.MembershipCategoryTotalLoans),
                   }
               ).Join(_context.Loans,
                   member => member.MemberNumber,
                   loan => loan.MemberNumber,
                   (member, loan) => new TotalDVDLoanViewModel
                   {
                       MembershipCategoryDescription = member.MembershipCategoryDescription,
                       MembershipCategoryTotalLoans = member.MembershipCategoryTotalLoans,
                       MemberNumber = member.MemberNumber,
                       FirstName = member.FirstName,
                       LastName = member.LastName,
                       Address = member.Address,
                       DateOfBirth = member.DateOfBirth,
                       LoanMemberId = loan.MemberNumber,
                       DateReturned = loan.DateReturned == null ? "" : loan.DateReturned.ToString(),
                   }
               )
               .Where(x => x.DateReturned == "" && x.MemberNumber == membernumber)
               .GroupBy(x => x.MemberNumber)
               .Select(x => new TotalDVDLoanViewModel
               {
                   //UID = x.Key,
                   //List = x.ToList(),

                   Total = x.Count(),
                   MemberNumber = x.Single().MemberNumber,
                   MembershipCategoryDescription = x.Single().MembershipCategoryDescription,
                   MembershipCategoryTotalLoans = x.Single().MembershipCategoryTotalLoans,
                   FirstName = x.Single().FirstName,
                   LastName = x.Single().LastName,
                   Address = x.Single().Address,
                   DateOfBirth = x.Single().DateOfBirth,
                   LoanMemberId = x.Single().LoanMemberId,
                   DateReturned = x.Single().DateReturned,
               })
               .OrderBy(x => x.FirstName)
               .ToList();
            

            foreach (var totalLoans in t2)
            {
             
                var memberTakenLoansTillNow = totalLoans.Total;
                
                var totalLoansAllowedForMembers = totalLoans.MembershipCategoryTotalLoans;
                //return Json(memberTakenLoansTillNow);
                if (totalLoansAllowedForMembers < memberTakenLoansTillNow)
                {
                    return Json("Sorry the members has taken loan of too Many DVDs");
                }
            }


            foreach (var loanItem in loanedOut3)
            {
                //return Json("here");
                var loanDays = loanItem.loantype;
                var dateOut = DateTime.Now;
                var dateDue = dateOut.AddDays(int.Parse(loanDays));
                //return Json(dateDue);
                Loan loans = new Loan();
                loans.MemberNumber = membernumber;
                loans.CopyNumber = copyNumber;
                loans.LoanTypeNumber = loanNumberrrr;
                loans.DateOut = dateOut;
                loans.DateDue = dateDue;
                //return Json(loans);
                _context.Add(loans);

                break;
            }
            await _context.SaveChangesAsync();
            //return Json(loanedOut);

            //_context.Add(loan);
            //    await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //if (ModelState.IsValid)
            //{
            //    _context.Add(loan);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CopyNumber"] = new SelectList(_context.DVDCopies, "CopyNumber", "CopyNumber", loan.CopyNumber);
            //ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanTypeNumber", loan.LoanTypeNumber);
            //ViewData["MemberNumber"] = new SelectList(_context.Members, "MemberNumber", "MemberNumber", loan.MemberNumber);
            return View(loan);
        }

        // GET: AddLoans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["CopyNumber"] = new SelectList(_context.DVDCopies, "CopyNumber", "CopyNumber", loan.CopyNumber);
            ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanTypeNumber", loan.LoanTypeNumber);
            ViewData["MemberNumber"] = new SelectList(_context.Members, "MemberNumber", "MemberNumber", loan.MemberNumber);
            return View(loan);
        }

        // POST: AddLoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoanNumber,LoanTypeNumber,CopyNumber,MemberNumber,DateOut,DateDue,DateReturned")] Loan loan)
        {
            if (id != loan.LoanNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.LoanNumber))
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
            ViewData["CopyNumber"] = new SelectList(_context.DVDCopies, "CopyNumber", "CopyNumber", loan.CopyNumber);
            ViewData["LoanTypeNumber"] = new SelectList(_context.LoanTypes, "LoanTypeNumber", "LoanTypeNumber", loan.LoanTypeNumber);
            ViewData["MemberNumber"] = new SelectList(_context.Members, "MemberNumber", "MemberNumber", loan.MemberNumber);
            return View(loan);
        }

        // GET: AddLoans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.DVDCopy)
                .Include(l => l.LoanType)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.LoanNumber == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: AddLoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.LoanNumber == id);
        }
    }
}
