using groupCW.Data;
using groupCW.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace groupCW.Controllers
{
    // Number 8
    public class TotalDVDLoanController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TotalDVDLoanController(ApplicationDbContext db)
        {
            _db = db;   
        }

        public IActionResult Index()
        {
            List<TotalDVDLoanViewModel> t2 = _db.MembershipCategories.Join(_db.Members,
                    mbscategory => mbscategory.MembershipCatagoryNumber,
                    members => members.MembershipCategoryNumber,
                    (mbscategory, members) => new TotalDVDLoanViewModel
                    {
                        MemberNumber = members.MemberNumber,
                        FirstName = members.MemberFirstName,
                        LastName = members.MemberLastName,
                        Address = members.MemberAddress,
                        DateOfBirth = members.MemberDateOfBirth,
                        MembershipCategoryDescription = mbscategory.MembershipCategoryDescription,
                        MembershipCategoryTotalLoans = mbscategory.MembershipCategoryTotalLoans == null ? 0 :mbscategory.MembershipCategoryTotalLoans,
                    }
                ).Join(_db.Loans,
                    member => member.MemberNumber,
                    loan => loan.MemberNumber,
                    (member, loan) => new TotalDVDLoanViewModel
                    {
                        MembershipCategoryDescription = member.MembershipCategoryDescription,
                        MembershipCategoryTotalLoans = member.MembershipCategoryTotalLoans,
                        MemberNumber = member.MemberNumber,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Address = member.Address,
                        DateOfBirth = member.DateOfBirth,
                        LoanMemberId = loan.MemberNumber,
                        DateReturned = loan.DateReturned == null ? "" : loan.DateReturned.ToString(),
                    }
                )
                .Where(x => x.DateReturned == "")
                .GroupBy(x => x.MemberNumber)
                .Select(x => new TotalDVDLoanViewModel
                {
                    Total = x.Count(),
                    MemberNumber = x.Single().MemberNumber,
                    MembershipCategoryDescription = x.Single().MembershipCategoryDescription,
                    MembershipCategoryTotalLoans = Int32.Parse(x.Single().MembershipCategoryTotalLoans.ToString()),
                    FirstName = x.Single().FirstName,
                    LastName = x.Single().LastName,
                    Address = x.Single().Address,
                    DateOfBirth = x.Single().DateOfBirth,
                    LoanMemberId = x.Single().LoanMemberId,
                    DateReturned = x.Single().DateReturned,
                })
                .ToList();

            List<TotalDVDLoanViewModel> t = t2.OrderBy(x => x.FirstName).ToList();

            // return Json(t2);

            return View(t2);
        }
    }
}
