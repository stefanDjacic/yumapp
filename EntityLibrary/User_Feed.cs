using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class User_Feed
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }

        //Navigation properties
        public int AppUserId { get; set; }
        //[ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        public int PostId { get; set; }
        public int PostAppUserId { get; set; }
        public Post Post { get; set; }
    }
}
