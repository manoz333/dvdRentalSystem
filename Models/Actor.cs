using System.ComponentModel.DataAnnotations;

namespace DatabaseCoursework.Models
{
    public class Actor
    {
        [Key]
        public int ActorNumber { get; set; }
        public string ActorSurname { get; set; }
        public string ActorFirstname { get; set; }          
       
    }
}
