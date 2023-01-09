using System.ComponentModel.DataAnnotations;

namespace DatabaseCoursework.Models
{
    public class MembershipCategory
    {
        [Key]
        public int MembershipCatagoryNumber { get; set; }
        public string? MembershipCategoryDescription { get; set; }

        // Total number of dvd loan taken by member user
        public int MembershipCategoryTotalLoans { get; set; }
    }
}
