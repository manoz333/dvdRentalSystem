using groupCW.Views.DVDSearch;

namespace groupCW.ViewModel
{
    public class TotalDVDLoanViewModel
    {
        public int MemberNumber { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Address { get; set; }

        public string DateOfBirth { get; set; }

        public int LoanMemberId { get; set; }

        public string MembershipCategoryDescription { get; set; }

        public int MembershipCategoryTotalLoans { get; set; }    

        public string DateReturned { get; set; }
        public int UID { get; internal set; }

        public List<TotalDVDLoanViewModel> List { get; internal set; }
        public int Total { get; internal set; }
    }
}
