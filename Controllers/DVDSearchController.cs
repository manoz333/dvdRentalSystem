using DatabaseCoursework.Models;
using groupCW.Data;
using groupCW.ViewModel;
using groupCW.Views.DVDSearch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace groupCW.Controllers
{
    public class DVDSearchController : Controller
    {
        private readonly ApplicationDbContext _db;

      

        public DVDSearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Number 1
        public IActionResult Index(string show)
        {
            JoinHelper j;
            j = new JoinHelper();

            if (show != null) {     
                j.show = show;
            } else
            {
                return View();
            }

            return View(j);
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

        // Number 2
        public IActionResult DVDWithAvailability(string show)
        {
            return View();
        }

        [HttpPost]
        public IActionResult FilterWithAvailability(string lName)
        {
            if (lName == null || lName.Trim() == "")
            {
                return RedirectToAction("DVDWithAvailability");
            }

            List<FilterWithAvailabilityViewModel> objDvdList = (
                    from actors in _db.Actors
                    join castmember in _db.CastMembers on actors.ActorNumber equals castmember.ActorNumber

                    join dvdtitles in _db.DVDTitles on castmember.DVDNumber equals dvdtitles.DVDNumber

                    join dvdcopies in _db.DVDCopies on dvdtitles.DVDNumber equals dvdcopies.DVDNumber
                    group dvdcopies by new { dvdNumber = dvdcopies.DVDNumber, dvdTitle = dvdtitles.DVDTitles, actorFName = actors.ActorFirstname,
                    actorLName = actors.ActorSurname
                    } into grp

                    select new FilterWithAvailabilityViewModel
                    {
                        dvdTitle = grp.Key.dvdTitle.ToString(),
                        fName = grp.Key.actorFName.ToString(),
                        lName = grp.Key.actorLName.ToString(),
                        dvdNumber = grp.FirstOrDefault().DVDNumber,
                        total = grp.Count(),
                        total2 = grp.Count()
                    }
                ).ToList();

            List<FilterWithAvailabilityViewModel> objDvdList2 = (
                    from dvdcopies in _db.DVDCopies
                    join loan in _db.Loans on dvdcopies.CopyNumber equals loan.CopyNumber
                    where loan.DateReturned == null
                    group dvdcopies by dvdcopies.DVDNumber into grp
                    select new FilterWithAvailabilityViewModel
                    {
                        dvdNumber = grp.FirstOrDefault().DVDNumber,
                        count = grp.Count()
                    }
                ).ToList();

            for (var i = 0; i < objDvdList.Count(); i++) {

                for (var j = 0; j < objDvdList2.Count(); j++) {

                    if (objDvdList[i].dvdNumber == objDvdList2[j].dvdNumber) { 
                        objDvdList[i].total = objDvdList[i].total - objDvdList2[j].count;
                    }
                
                }

            }

            List<FilterWithAvailabilityViewModel> result = objDvdList.Where(x => x.lName.ToLower() == lName.ToLower()).ToList();


            return View(result);

            //return Json(objDvdList);
        }


    }
}
