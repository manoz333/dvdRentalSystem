using groupCW.Data;
using groupCW.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    public class RemoveOldDVDController : Controller
    {
        private readonly ApplicationDbContext _db;
        public RemoveOldDVDController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<RemoveOldDVDViewModel> objDvdList = _db.DVDCopies.Join(_db.DVDTitles,
                    copies => copies.DVDNumber, dvd => dvd.DVDNumber,
                    (copies, dvd) => new
                    {
                        dTitle = dvd.DVDTitles,
                        dvdid = dvd.DVDNumber,
                        copyNumber = copies.CopyNumber,
                        dvdReleasedDate = dvd.DateReleased,
                        dvdDatePurchased = copies.DatePurchased,

                    }
                ).Join(_db.Loans,
                    copies => copies.copyNumber, loan => loan.CopyNumber,
                    (copies, loan) => new RemoveOldDVDViewModel()
                    {
                        dvdid = copies.dvdid,
                        dvdTitle = copies.dTitle,
                        dvdReleaseDate = copies.dvdReleasedDate,
                        copyNumber = copies.copyNumber,
                        dvdDatePurchased = copies.dvdDatePurchased,
                        dvdDateReturned = loan.DateReturned,
                        dvdDateOut = loan.DateOut,

                    }).Where(x => x.dvdDatePurchased <= DateTime.Now.AddDays(-365) && x.dvdDateReturned != null)
                    .ToList();

            List<RemoveOldDVDViewModel> objDvdList2 = objDvdList.DistinctBy(x => x.copyNumber).ToList();

            List<RemoveOldDVDViewModel> copies = _db.DVDCopies.Join(_db.DVDTitles,
                    copies => copies.DVDNumber, dvd => dvd.DVDNumber,
                    (copies, dvd) => new RemoveOldDVDViewModel
                    {
                        dvdTitle = dvd.DVDTitles,
                        dvdid = dvd.DVDNumber,
                        copyNumber = copies.CopyNumber,
                        dvdReleasedDate = dvd.DateReleased,
                        dvdDatePurchased = copies.DatePurchased,

                    }
                )
                .Where(x => x.dvdDatePurchased <= DateTime.Now.AddDays(-365))
                .ToList();

            List<RemoveOldDVDViewModel> loans = _db.Loans.Where(x => x.DateReturned != null)
                .Select(x => new RemoveOldDVDViewModel { 
                    copyNumber = x.CopyNumber,
                    dvdDateReturned = x.DateReturned,
                })
                .ToList();

            List<RemoveOldDVDViewModel> test = new List<RemoveOldDVDViewModel>();

            for (var i = 0; i < copies.Count(); i++) {
                var count = 0;
                for (var j = 0; j < loans.Count(); j++) {
                    if (copies[i].copyNumber == loans[j].copyNumber) {
                        copies[i].dvdDateReturned = loans[j].dvdDateReturned;
                        test.Add(copies[i]);
                    }

                    if (copies[i].copyNumber == loans[j].copyNumber) {
                        count++;   
                    }
                }

                if (count == 0) { 
                    test.Add(copies[i]);
                    count = 0;
                }
            }


            return View(test);
        }

        public  IActionResult RemoveDVD (int id)
        {
            _db.Remove(_db.DVDCopies.Single(x => x.CopyNumber == id));

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
