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
        [Column(TypeName = "date")]
        public DateTime DateOfFollowing { get; set; } = DateTime.UtcNow;

        //Navigation properties
        public int FollowerId { get; set; }        
        public AppUser Follower { get; set; }

        public int FollowsId { get; set; }        
        public AppUser Follows { get; set; }
    }
}
