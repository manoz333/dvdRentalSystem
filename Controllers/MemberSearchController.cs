using groupCW.Data;
using groupCW.Views.DVDSearch;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    public class MemberSearchController : Controller
    {
        private readonly ApplicationDbContext _db;
        public MemberSearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            DateTime currentDate = DateTime.Now.Date;

            DateTime lastDate = currentDate.Subtract(new TimeSpan(31, 0, 0, 0, 0));

            List<JoinHelper> objDvdList = _db.Members.Join(_db.Loans,
                mem => mem.MemberNumber, loan => loan.MemberNumber,
                (mem, loan) => new
                {
                    loan = loan.CopyNumber,
                    dateOut = loan.DateOut,
                    lName = mem.MemberLastName,
                    fName = mem.MemberFirstName,

                })
                .Join(_db.DVDCopies,
                loans => loans.loan, copies => copies.CopyNumber,
                (loans, copies) => new
                {
                    fName = loans.fName,
                    lName = loans.lName,
                    dateOut = loans.dateOut,

                    copyNumber = copies.DVDNumber,

                }
            ).Join(_db.DVDTitles,
                dvdcopies => dvdcopies.copyNumber, dvdtitle => dvdtitle.DVDNumber,
                (dvdcopies, dvdtitle) => new JoinHelper()
                {
                    fName = dvdcopies.fName,
                    lName = dvdcopies.lName,
                    dateOut = dvdcopies.dateOut,
                    copyNumber = dvdcopies.copyNumber,
                    dvdtitle = dvdtitle.DVDTitles,
                    dvdNumberId = dvdtitle.DVDNumber,
                }
            ).ToList();

            JoinList jl = new JoinList();

            jl.showAllMember = false;
            jl.JoinHelperList = objDvdList;

            return View(jl);
        }

        [HttpPost]
        public IActionResult Index(string memberLastName)
        {
            if (memberLastName == null || memberLastName.Trim() == "")
            {
                return RedirectToAction("Index");
            }


            DateTime currentDate = DateTime.Now.Date;

            DateTime lastDate = currentDate.Subtract(new TimeSpan(31, 0, 0, 0, 0));

            List<JoinHelper> objDvdList = _db.Members.Join(_db.Loans,
                mem => mem.MemberNumber, loan => loan.MemberNumber,
                (mem, loan) => new
                {
                    loan = loan.CopyNumber,
                    dateOut = loan.DateOut,
                    lName = mem.MemberLastName,
                    fName = mem.MemberFirstName,

                })
                .Join(_db.DVDCopies,
                loans => loans.loan, copies => copies.CopyNumber,
                (loans, copies) => new
                {
                    fName = loans.fName,
                    lName = loans.lName,
                    dateOut = loans.dateOut,

                    copyNumber = copies.DVDNumber,
                    
                }
            ).Join(_db.DVDTitles,
                dvdcopies => dvdcopies.copyNumber, dvdtitle => dvdtitle.DVDNumber,
                (dvdcopies, dvdtitle) => new JoinHelper()
                {
                    fName = dvdcopies.fName,
                    lName = dvdcopies.lName,
                    dateOut = dvdcopies.dateOut,
                    copyNumber = dvdcopies.copyNumber,
                    dvdtitle = dvdtitle.DVDTitles,
                    dvdNumberId = dvdtitle.DVDNumber,
                    releaseDate2 = dvdtitle.DateReleased.ToString()
                }
            ).Where(x => x.lName.ToLower() == memberLastName.ToLower() & x.dateOut <= lastDate).ToList();

            JoinList jl = new JoinList();

            jl.showAllMember = true;
            jl.memberLastName = memberLastName;
            jl.JoinHelperList = objDvdList;

            return View(jl);


        }
    }
}
