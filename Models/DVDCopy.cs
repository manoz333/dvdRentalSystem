using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DatabaseCoursework.Models
{
    public class DVDCopy
    {
        [Key]
        public int CopyNumber { get; set; } 

        public int DVDNumber { get; set; }

        public DateTime? DatePurchased { get; set; }
    }
}
