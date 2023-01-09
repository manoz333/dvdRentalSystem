namespace groupCW.Views.DVDSearch
{
    public class JoinList
    {

        // For number 3
        public string memberLastName { get; set; }  
        
        public List<JoinHelper> JoinHelperList { get; set; }

        public bool showAllMember { get; set; } = false;

        // for no 5
        public bool showTableData { get; set; }

        public string copyNumber { get; set; }  
    }
}
