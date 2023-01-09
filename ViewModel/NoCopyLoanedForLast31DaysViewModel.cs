namespace groupCW.ViewModel
{
    public class NoCopyLoanedForLast31DaysViewModel
    {
        public int? dvdNumber { get; set; }

        public int? copyNumber { get; set; }

        public string? dvdTitle { get; set; }

        public int total { get; set; }

        public DateTime? dateOut { get; set; }

        public DateTime? currentDate { get; set; }

        public string? noOfDaysSinceLastLoan { get; set; }

        public List<NoCopyLoanedForLast31DaysViewModel> records { get; set; }
    }
}
