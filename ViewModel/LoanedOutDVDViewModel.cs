namespace groupCW.ViewModel
{
    public class LoanedOutDVDViewModel
    {
        public int TotalNumberOfLoans { get; set; }
        public int copyNumber { get; set; }

        public int dvdNumber { get; set; }
        public DateTime? dateOut { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public int? memberNumber { get; set; }

        public DateTime? dvdReleaseDate { get; set; }
        public DateTime? dateDue { get; set; }

        public string? dateDueStringOnlyDate { get; set; }


        public DateTime? datePurchased { get; set; }
        public DateTime? dateReturned { get; set; }
        public string? dvdTitle { get; set; }
    }
}
