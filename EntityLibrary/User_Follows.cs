using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class User_Follows
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfFollowing { get; set; } = DateTime.UtcNow;

        //Navigation properties
        public int FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public AppUser Follower { get; set; }

        public int FollowsId { get; set; }
        [ForeignKey("FollowsId")]
        public AppUser Follows { get; set; }
    }
}
