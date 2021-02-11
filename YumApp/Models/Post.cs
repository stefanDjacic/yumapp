using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        
        [Required]
        public string Content { get; set; }
        public DateTime TimeOfPosting { get; set; } = DateTime.Now.ToLocalTime();

        //Navigation properties
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        
    }
}
