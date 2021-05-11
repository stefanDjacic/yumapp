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
        //Navigation properties
        public int AppUserId { get; set; }        
        public AppUser AppUser { get; set; }

        public int PostId { get; set; }
        public int PostAppUserId { get; set; }
        public Post Post { get; set; }
    }
}
