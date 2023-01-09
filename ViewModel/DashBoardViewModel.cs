namespace groupCW.ViewModel
{
    public class DashBoardViewModel
    {
        public List<DashBoardViewModel>? DashBoardViewModels { get; set; }
        public List<string>? Categories { get; set; }
        public int? TotalDVDCopiesOnLoan { get; set; }
        public int? TotalMovies { get; set; }
        public List<string>? Members { get; set; }
    }
}
