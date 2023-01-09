using DatabaseCoursework.Models;
using groupCW.Data;
using groupCW.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    public class ShowAllDVDController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ShowAllDVDController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            List<ShowAllViewModel> list = (
                        from dvdtitles in _db.DVDTitles

                        join dvdcopy in _db.DVDCopies on dvdtitles.DVDNumber equals dvdcopy.DVDNumber
                        join loan in _db.Loans on dvdcopy.CopyNumber equals loan.CopyNumber into DVDCopyLoanGroup

                        from test in DVDCopyLoanGroup.DefaultIfEmpty()

                        join loantype in _db.LoanTypes on test.LoanTypeNumber equals loantype.LoanTypeNumber into grp

                        from t in grp.DefaultIfEmpty()

                        select new ShowAllViewModel {
                            
                            DvdTitle = dvdtitles.DVDTitles,
                            PenaltyCharge = Int32.Parse(dvdtitles.PenaltyCharge),
                            CopyNumber = dvdcopy.CopyNumber.ToString(),
                            LoanNumber = test.LoanNumber,
                            ReturnDate = test.DateReturned,
                            DateOut = test.DateOut,
                            DueDate = test.DateDue,
                            LoanType = t.LoanTypes,

                            NoOfDaysDVDNotReturnedAfterDeadline = test.DateReturned == null || test.DateDue == null ? "" :  (test.DateReturned - test.DateDue).Value.ToString(),


                        }).ToList();



            return View(list);
        }

        public IActionResult UpdateReturnDate(int loanid)
        {
            Loan l = _db.Loans.Where(x => x.LoanNumber == loanid).FirstOrDefault();

            if (l == null)
            {
                return Content("Update Failed, Please try again later!");
            }

            l.DateReturned = DateTime.Now;

            _db.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}
