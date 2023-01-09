using groupCW.Data;
using groupCW.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashBoardController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            DashBoardViewModel d = new DashBoardViewModel();

            

            List<string> categories = _db.DVDCategories.Select(category =>  category.CategoryDescription).ToList();

            List<string> uniqueCategories = categories.Distinct().ToList();

            List<string> members = _db.Members.Select(member => member.MemberFirstName + " " + member.MemberLastName).ToList();

            int dvdOnLoan = _db.Loans.Where(x => x.DateReturned == null).ToList().Count();

            int movieCount = _db.DVDTitles.Select(x => x.DVDTitles).Distinct().ToList().Count();

            d.Categories = uniqueCategories;
            d.Members = members;
            d.TotalDVDCopiesOnLoan = dvdOnLoan;
            d.TotalMovies = movieCount;

            return View(d);
        }
    }
}
