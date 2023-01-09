using System.ComponentModel.DataAnnotations;

namespace DatabaseCoursework.Models
{
    public class LoanType
    {
        [Key]
        public int LoanTypeNumber { get; set; }
        public string LoanTypes { get; set; }
        public string LoanDuration { get; set; }

    }
}
