namespace groupCW.ViewModel
{
    public class RemoveOldDVDViewModel
    {
        internal DateTime? dvdReleasedDate;
        internal string dTitle;

        public DateTime? dvdReleaseDate { get; set; }
        public DateTime? dvdDateOut { get; set; }
        public DateTime? dvdDatePurchased { get; set; }
        public DateTime? dvdDateReturned { get; set; }
        public string? dvdTitle { get; set; }

        public int? dvdid { get; set; }
        public int copyNumber { get; set; }
    }
}
