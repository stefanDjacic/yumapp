using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string NotificationText { get; set; }

        //Navigation Properties
        public int AppUserId { get; set; }
        //[ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; }
        
        public int DoerId { get; set; }
        //[ForeignKey(nameof(DoerId))]
        public AppUser Doer { get; set; }
    }
}
