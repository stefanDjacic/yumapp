using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public class User_Follows
    {        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.ToLocalTime();

        //Navigation properties
        public int FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public AppUser Follower { get; set; }

        public int FollowsId { get; set; }
        [ForeignKey("FollowsId")]
        public AppUser Follows { get; set; }
    }
}
