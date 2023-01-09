using groupCW.Data;
using groupCW.Views.DVDSearch;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    public class Number1Controller : Controller
    {
        private readonly ApplicationDbContext _db;
        public Number1Controller(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Filter(string lName)
        {
            if (lName == null || lName.Trim() == "")
            {
                return RedirectToAction("Index");
            }

            IEnumerable<JoinHelper> objDvdList = _db.DVDTitles.Join(_db.CastMembers,
                 dvdtitles => dvdtitles.DVDNumber, castmem => castmem.DVDNumber,
                 (dvdtitles, castmem) => new
                 {
                     dTitle = dvdtitles.DVDTitles,
                     castmemberid = castmem.DVDNumber,
                     actoriden = castmem.ActorNumber,
                     releaseDate = dvdtitles.DateReleased
                 }
                 ).Join(_db.Actors, castmeme => castmeme.actoriden, act => act.ActorNumber,
                 (castmeme, act) => new JoinHelper
                 {
                     fName = act.ActorFirstname,
                     lName = act.ActorSurname,
                     castMemberId = castmeme.castmemberid,
                     dTitleName = castmeme.dTitle,
                     releaseDate2 = castmeme.releaseDate.ToString(),
                 }
                 ).Where(x => x.lName.ToLower() == lName.ToLower()).ToList();



            return View(objDvdList);
        }
    }
}
