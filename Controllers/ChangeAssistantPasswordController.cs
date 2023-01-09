using groupCW.Data;
using groupCW.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    public class ChangeAssistantPasswordController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ChangeAssistantPasswordController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            List<AspUserViewModel> l =  _db.Users.Select(x => new AspUserViewModel {
                Id = x.Id,
                UserName  = x.UserName,
                Email = x.Email,
            }).ToList();

            return View(l);
        }

        public IActionResult IndividualUser(string id)
        {
            List<AspUserViewModel> l = _db.Users.Join(_db.UserRoles,
                    users => users.Id, userroles => userroles.UserId,
                    (users, userroles) => new AspUserViewModel { 
                        RoleId = userroles.RoleId,
                        UserId = users.Id,  
                    }
                ).ToList();

            return View();
        }

    }
}
