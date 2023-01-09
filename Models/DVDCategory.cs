using System.ComponentModel.DataAnnotations;

namespace DatabaseCoursework.Models
{
    public class DVDCategory
    {
        [Key]
        public int CategoryNumber { get; set; }
        public string CategoryDescription { get; set; }

        public string AgeRestricted { get; set; }
    }
}
