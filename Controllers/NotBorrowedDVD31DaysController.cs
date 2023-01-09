using groupCW.Data;
using groupCW.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace groupCW.Controllers
{
    // number 12
    public class NotBorrowedDVD31DaysController : Controller
    {
        private readonly ApplicationDbContext _db;
        public NotBorrowedDVD31DaysController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(NotBorrowedDVDLast31DaysViewModel x)
        {
            List<NotBorrowedDVDLast31DaysViewModel> memberList = _db.Members.Join(_db.Loans, 
                members => members.MemberNumber, loans => loans.MemberNumber,
                (members, loans) => new NotBorrowedDVDLast31DaysViewModel
                {
                    memberFirstName = members.MemberFirstName,
                    memberLastName = members.MemberLastName,
                    memberAddress = members.MemberAddress,
                    dateOut = loans.DateOut,
                    copyNumber = loans.CopyNumber,
                }
                )
                .Join(
                    _db.DVDCopies,
                    loans => loans.copyNumber, dvdcopies => dvdcopies.CopyNumber,
                    (loans, dvdcopies) => new NotBorrowedDVDLast31DaysViewModel
                    {
                        memberFirstName = loans.memberFirstName,
                        memberLastName = loans.memberLastName,
                        memberAddress = loans.memberAddress,
                        dateOut = loans.dateOut,
                        copyNumber = loans.copyNumber,
                        dvdNumber = dvdcopies.DVDNumber
                    }
                )
                .Join(
                    _db.DVDTitles,
                    dvdcopies => dvdcopies.dvdNumber, dvdtitle => dvdtitle.DVDNumber,
                    (dvdcopies, dvdtitles) => new NotBorrowedDVDLast31DaysViewModel
                    {
                        memberFirstName = dvdcopies.memberFirstName,
                        memberLastName = dvdcopies.memberLastName,
                        memberAddress = dvdcopies.memberAddress,
                        dateOut = dvdcopies.dateOut,
                        copyNumber = dvdcopies.copyNumber,
                        dvdNumber = dvdcopies.dvdNumber,
                        dvdTitle = dvdtitles.DVDTitles
                    }
                )
                .Where(
                    x => (DateTime.Now.AddDays(-31) >= x.dateOut)
                )
                .ToList();

            return View(memberList);

        }
    }
}
