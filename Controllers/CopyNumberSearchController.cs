using groupCW.Data;
using groupCW.Views.DVDSearch;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    public class CopyNumberSearchController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CopyNumberSearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            List<JoinHelper> objDvdList = _db.Members.Join(_db.Loans,
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
                        copyNumber = copies.DVDNumber,

                    }
                ).Join(_db.DVDTitles,
                    dvdcopies => dvdcopies.copyNumber, dvdtitle => dvdtitle.DVDNumber,
                    (dvdcopies, dvdtitle) => new JoinHelper()
                    {
                        memberNumber = dvdcopies.memberNumber,
                        fName = dvdcopies.fName,
                        lName = dvdcopies.lName,
                        dateOut = dvdcopies.dateOut,
                        dateDue = dvdcopies.dateDue,
                        dateReturned = dvdcopies.dateReturned,
                        copyNumber = dvdcopies.copyNumber,
                        dvdtitle = dvdtitle.DVDTitles,
                    }
                ).ToList();

            JoinList jl = new JoinList();


            jl.showTableData = false;
            jl.JoinHelperList = objDvdList;

            return View(jl);

        }


        [HttpPost]
        public IActionResult Index(string copyNumber)
        {
            int no;

            try
            {
                no = int.Parse(copyNumber);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Content("Enter a valid dvd copy number!!!");
            }

            List<JoinHelper> objDvdList = _db.Members.Join(_db.Loans,
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
                        copyNumber = copies.CopyNumber,

                    }
                ).Join(_db.DVDTitles,
                    dvdcopies => dvdcopies.copyNumber, dvdtitle => dvdtitle.DVDNumber,
                    (dvdcopies, dvdtitle) => new JoinHelper()
                    {
                        memberNumber = dvdcopies.memberNumber,
                        fName = dvdcopies.fName,
                        lName = dvdcopies.lName,
                        dateOut = dvdcopies.dateOut,
                        dateDue = dvdcopies.dateDue,
                        dateReturned = dvdcopies.dateReturned,
                        copyNumber = dvdcopies.copyNumber,
                        dvdtitle = dvdtitle.DVDTitles,
                    }
                ).Where(x => x.copyNumber == no)
                .ToList();

            List<int> copyNumbers = _db.DVDCopies.Select(x => x.CopyNumber).ToList();

            var x = Int32.Parse(copyNumber);

            var validCopyNumbers = "";

            if (copyNumbers.IndexOf(x) == -1 )
            {
                for(int i = 0; i < copyNumbers.Count(); i++)
                {
                    validCopyNumbers += ", " + copyNumbers[i];
                }

                return Content("There are no copies with copy number " + copyNumber + ". | Valid copy numbers : " + validCopyNumbers.Substring(1));

            }

            if (objDvdList.Count() <= 0)
            {
                return Content("Copy number " + copyNumber + " has yet not been loaned, so there is no last loan date for copy number " + copyNumber);

            }

            DateTime latestOutDate = DateTime.Now;

            for (var i = 0; i < 1;)
            {
                latestOutDate = (DateTime) objDvdList[0].dateOut;

                for (var j = 0; j < objDvdList.Count; j++)
                {
                    if (objDvdList[j].dateOut >= latestOutDate)
                    {
                        latestOutDate = (DateTime) objDvdList[j].dateOut;
                    }
                }

                break;
            }


            var test = objDvdList.Where(x => x.dateOut == latestOutDate).ToList();

            JoinList jl = new JoinList();


            jl.showTableData = true;
            jl.JoinHelperList = test;

            return View(jl);

        }
    }
}
