using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseCoursework.Models
{
    public class CastMember
    {
        public int DVDNumber { get; set; }

        public int ActorNumber { get; set; }

        [ForeignKey("DVDNumber")]
        public  DVDTitle DVDTitle{ get; set; }

        [ForeignKey("ActorNumber")]
        public Actor Actor{ get; set; }

    }
}
