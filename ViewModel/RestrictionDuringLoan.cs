namespace groupCW.ViewModel
{
    public class RestrictionDuringLoan
    {
        public string memberfName { get; set; }
        public int memberNumber { get; set; }
        public int? membershipTotalLoan { get; set; }
        public string memberlName { get; set; }
        public DateTime? dateOut { get; set; }
        public DateTime? dateDue { get; set; }
        public DateTime? dateReturned { get; set; }

        public string memberDateOfBirth { get; set; }
        public int copyNumber { get; set; }
        public int dvdNumber { get; set; }
        public string dvdTitle { get; set; }
        public int dvdCategory { get; set; }
        public string ageRestricted { get; set; }
        public string membershipDesc { get; set; }
        public int? loanduration { get; set; }
        public string loantype { get; set; }
    }
}
