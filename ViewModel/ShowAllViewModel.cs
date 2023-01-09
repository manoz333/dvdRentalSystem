namespace groupCW.ViewModel
{
    public class ShowAllViewModel
    {
        public string? MemberName { get; set; }
        public string? DvdTitle { get; set; }   

        public DateTime? DateOut { get; set; }  

        public DateTime? DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public string? NoOfDaysDVDNotReturnedAfterDeadline { get; set; }

        public int? TotalPenalty { get; set; }

        public string? LoanType { get; set; }   

        public string? LoanDuration { get; set; }

        public int? PenaltyCharge { get; set; }  

        // for table joining purposes

        public int? DvdNumber { get; set; }
        public string? CopyNumber { get; set; }
        public int? LoanTypeNumber { get; set; }
        public int? LoanNumber { get; set; }
        public int? MemberNumber { get; set; }
    }
}
