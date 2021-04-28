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

        [Required]
        public DateTime TimeOfNotification { get; set; }

        //Navigation Properties
        public int ReceiverId { get; set; }
        public AppUser Receiver { get; set; }
        
        public int InitiatorId { get; set; }
        public AppUser Initiator { get; set; }
    }
}
