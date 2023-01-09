namespace groupCW.ViewModel
{
    public class FilterWithAvailabilityViewModel
    {
        public string fName { get; set; }
        public string lName { get; set; }
        public string? dvdReturnedDate { get; set; }

        public string? dvdTitle { get; set; }
        public int? dvdNumber { get; set; }

        public string? copyNumber { get; set; }

        public string? loanNumber { get; set; }
        public string? dateOut { get; set; } 

        public string? loanCopyNumber { get; set; } 

        public int? total { get; set; }

        public int? total2 { get; set; }


        public int? count { get; set; }


        public IEnumerable<FilterWithAvailabilityViewModel> list { get; set; }  
    }
}
