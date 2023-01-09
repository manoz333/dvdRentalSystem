namespace groupCW.Views.DVDSearch
{
    public class JoinHelper
    {
        internal string dvdTitle;

        // This class was created to supply multiple models in a view
        public string fName { get; set; }
        public string lName { get; set; }
        public int castMemberId { get; set; }

        public int actoriden { get; set; }
        public int dvdNumberId { get; set; }
        public string dTitleName { get; set; }
        
        public int dNumber { get; set; }
        public string releaseDate2 { get; set; }

        public int dvdCopyNumber { get; set; }

        public int NumberOfCopies { get; set; }

        public int copyId { get; set; }

        public string show { get; set; }

        public int copyNumber { get; set; }

        public DateTime? dateOut { get; set; }

        public int loanNumber { get; set; }

        public DateTime? dvdCopyDatePurchased { get; set; }

        public DateTime? dvdReturnedDate { get; set; }


        // Used for number 4
        public int dvdId { get; set; }
        public string? producerName { get; set; }
        public string studioName { get; set; }

        public string actorFirstName { get; set; }
        public string actorLastName { get; set; }

        // Used for number 5

        public string dvdtitle { get; set; }
        public int memberNumber { get; set; }
        
        public DateTime? dateDue { get; set; }
       
        public DateTime? dateReturned { get; set; }


        // 8
        public int MemberNumbers { get; set; }
        public string FirstNames { get; set; }

        public string LastNames { get; set; }
        public string Addresss { get; set; }

        public string DateOfBirths { get; set; }

        public int LoanMemberId { get; set; }

        public string DateReturneds { get; set; }
        public int UID { get; internal set; }
        public List<JoinHelper> List { get; internal set; }
        public int Total { get; internal set; }



    }


}
