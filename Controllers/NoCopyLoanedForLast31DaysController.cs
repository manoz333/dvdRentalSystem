using groupCW.Data;
using groupCW.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    public class NoCopyLoanedForLast31DaysController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NoCopyLoanedForLast31DaysController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(IEnumerable<NoCopyLoanedForLast31DaysViewModel> x)
        {
            List<NoCopyLoanedForLast31DaysViewModel> dvdList = _db.Loans
                .Join(
                    _db.DVDCopies,
                    loans => loans.CopyNumber, dvdcopies => dvdcopies.CopyNumber,
                    (loans, dvdcopies) => new NoCopyLoanedForLast31DaysViewModel
                    {
                        dvdNumber = dvdcopies.DVDNumber,
                        dateOut = loans.DateOut,
                        copyNumber = dvdcopies.CopyNumber
                    }
                )
                .Join(
                    _db.DVDTitles,
                    dvdcopies => dvdcopies.dvdNumber, dvdtitle => dvdtitle.DVDNumber,
                    (dvdcopies, dvdtitle) => new NoCopyLoanedForLast31DaysViewModel
                    {
                        dvdNumber = dvdcopies.dvdNumber,
                        dvdTitle = dvdtitle.DVDTitles,
                        dateOut = dvdcopies.dateOut,
                        copyNumber = dvdcopies.copyNumber
                    }
                )
                .Where(
                    x => (DateTime.Now.AddDays(-31) >= x.dateOut)
                )
                .GroupBy(
                    x => x.dvdTitle
                ).
                Select(
                    x => new NoCopyLoanedForLast31DaysViewModel
                    {
                        total = x.Count(),
                        dvdNumber = x.Single().dvdNumber,
                        dvdTitle = x.Single().dvdTitle,
                        dateOut = x.Single().dateOut,
                        copyNumber = x.Single().copyNumber,
                        records = x.ToList(),
                        currentDate = DateTime.Now,
                        noOfDaysSinceLastLoan =  (DateTime.Now - x.Single().dateOut).Value.Days.ToString(),
                    }
                )
                .ToList();

            return View(dvdList);
        }
    }
}
