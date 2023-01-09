using groupCW.Data;
using groupCW.Views.DVDSearch;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    public class CastMemberSearchController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CastMemberSearchController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<JoinHelper> castMembers = _db.DVDTitles.Join(_db.Studios,
                dvdtitles => dvdtitles.StudioNumber, studio => studio.StudioNumber,
                (dvdtitles, studio) => new
                {
                    dTitle = dvdtitles.DVDTitles,
                    producerNumber = dvdtitles.ProducerNumber,
                    dvdId = dvdtitles.DVDNumber,
                    dvdDateReleased = dvdtitles.DateReleased,
                    studioName = studio.StudioName,
                }
            ).Join(_db.Producers, dvdtit => dvdtit.producerNumber, producer => producer.ProducerNumber,
                (dvdtit, producer) => new
                {
                    dvdId = dvdtit.dvdId,
                    dvdTitle = dvdtit.dTitle,
                    producerName = producer.ProducerName,
                    studioName = dvdtit.studioName,


                }
            ).Join(_db.CastMembers, dvdtitl => dvdtitl.dvdId, casts => casts.DVDNumber,
                (dvdtitl, casts) => new
                {
                    dvdId = dvdtitl.dvdId,
                    dvdTitle = dvdtitl.dvdTitle,
                    producerName = dvdtitl.producerName,
                    studioName = dvdtitl.studioName,
                    actorId = casts.ActorNumber,
                }

            ).Join(_db.Actors, dvdtitle => dvdtitle.actorId, actors => actors.ActorNumber,
                (dvdtitle, actors) => new JoinHelper()
                {
                    dvdTitle = dvdtitle.dvdTitle,
                    dvdId = dvdtitle.dvdId,
                    producerName = dvdtitle.producerName,
                    studioName = dvdtitle.studioName,
                    actorFirstName = actors.ActorFirstname,
                    actorLastName = actors.ActorSurname,
                }
            );

            return View(castMembers);
        }
    }
}
