using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class Yummy_Post
    {
        [Column(TypeName = "date")]
        public DateTime DateYummed { get; set; }

        //Navigation properties
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int PostId { get; set; }
        public int PostAppUserId { get; set; }
        public Post Post { get; set; }
    }
}
