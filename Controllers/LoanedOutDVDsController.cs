using groupCW.Data;
using groupCW.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace groupCW.Controllers
{
    // Number 11
    public class LoanedOutDVDsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LoanedOutDVDsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<LoanedOutDVDViewModel> loanedOut = _db.Members.Join(_db.Loans,
                    mem => mem.MemberNumber, loan => loan.MemberNumber,
                    (mem, loan) => new
                    {
                        memberNumber = mem.MemberNumber,
                        loan = loan.CopyNumber,
                        dateOut = loan.DateOut,
                        dateDue = loan.DateDue,
                        dateReturned = loan.DateReturned,
                        lName = mem.MemberLastName,
                        fName = mem.MemberFirstName,

                    })
                .Join(_db.DVDCopies,
                    loans => loans.loan, copies => copies.CopyNumber,
                    (loans, copies) => new
                    {
                        memberNumber = loans.memberNumber,
                        fName = loans.fName,
                        lName = loans.lName,
                        dateOut = loans.dateOut,
                        dateDue = loans.dateDue,
                        dateReturned = loans.dateReturned,
                        dvdNumber = copies.DVDNumber,
                        cpyNumber = copies.CopyNumber
                    }
                ).Join(_db.DVDTitles,
                    dvdcopies => dvdcopies.dvdNumber, dvdtitle => dvdtitle.DVDNumber,
                    (dvdcopies, dvdtitle) => new LoanedOutDVDViewModel()
                    {
                        memberNumber = dvdcopies.memberNumber,
                        fName = dvdcopies.fName,
                        lName = dvdcopies.lName,
                        dateOut = dvdcopies.dateOut,
                        dateDue = dvdcopies.dateDue,
                        dateReturned = dvdcopies.dateReturned,
                        dvdNumber = dvdcopies.dvdNumber,
                        copyNumber = dvdcopies.cpyNumber,
                        dvdTitle = dvdtitle.DVDTitles,
                    }
                )
                .Where(x => x.dateReturned == null)
                .GroupBy(x => x.dateOut )
                .Select(x => new LoanedOutDVDViewModel
                {
                    TotalNumberOfLoans = x.Count(),
                    dateOut = x.Single().dateOut,
                    dvdTitle = x.Single().dvdTitle,
                    copyNumber = x.Single().copyNumber,
                    fName = x.Single().fName,
                    lName = x.Single().lName,
                })
                .OrderBy(x => x.dateOut)
                .ToList();


            return View(loanedOut);
        }

        public IActionResult SalesInGivenDate(DateTime date)
        {
            List<LoanedOutDVDViewModel> loanedOut = _db.Members.Join(
                    _db.Loans,
                    member => member.MemberNumber, loans => loans.MemberNumber,
                    (member, loans) => new LoanedOutDVDViewModel
                    {
                        memberNumber = member.MemberNumber,
                        copyNumber = loans.CopyNumber,
                        dateOut = loans.DateOut,
                        dateDue = loans.DateDue,
                        dateReturned = loans.DateReturned,
                        fName = member.MemberFirstName,
                        lName = member.MemberLastName,
                    }
                )
                .Join(_db.DVDCopies,
                    loans => loans.copyNumber, dvdCopies => dvdCopies.CopyNumber,
                    (loans, dvdCopies) => new LoanedOutDVDViewModel
                    {
                        memberNumber = loans.memberNumber,
                        copyNumber = loans.copyNumber,
                        dateOut = loans.dateOut,
                        dateDue= loans.dateDue,
                        dateReturned= loans.dateReturned,
                        fName = loans.fName,
                        lName= loans.lName,
                        datePurchased = dvdCopies.DatePurchased,
                        dvdNumber = dvdCopies.DVDNumber
                    }
                )
                .Join(_db.DVDTitles,
                    dvdCopies => dvdCopies.dvdNumber, dvdTitles => dvdTitles.DVDNumber,
                    (dvdCopies, dvdTitles) => new LoanedOutDVDViewModel
                    {
                        memberNumber = dvdCopies.memberNumber,
                        copyNumber = dvdCopies.copyNumber,
                        dateOut = dvdCopies.dateOut,
                        dateDue = dvdCopies.dateDue,
                        dateReturned = dvdCopies.dateReturned,
                        fName = dvdCopies.fName,
                        lName = dvdCopies.lName,
                        datePurchased = dvdCopies.datePurchased,
                        dvdNumber = dvdCopies.dvdNumber,
                        dvdTitle = dvdTitles.DVDTitles,
                        dateDueStringOnlyDate = dvdCopies.dateOut.ToString()
                    }
                )
                .Where(x => x.dateReturned == null)
                .ToList();

            var o = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture.DateTimeFormat);


            List<LoanedOutDVDViewModel> loanedOut2 = loanedOut.Where(x => x.dateDueStringOnlyDate.Split(' ')[0] == o).ToList();


            return View(loanedOut2);
        }
    }
}
