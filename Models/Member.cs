using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseCoursework.Models
{
    public class Member 
    { 
    
        [Key]
        
        public int MemberNumber { get; set; }
        public int MembershipCategoryNumber { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberAddress { get; set; }
        public string MemberDateOfBirth { get; set; }

        [ForeignKey("MembershipCategoryNumber")]
        public MembershipCategory MemebershipCategory { get; set; }

    }

}
