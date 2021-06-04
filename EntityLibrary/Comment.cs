using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Minimum lenght is 1 character.")]
        [MaxLength(500, ErrorMessage = "Maximum lenght is 500 characters.")]
        public string Content { get; set; }

        public DateTime TimeOfCommenting { get; set; }

        //Navigation properties
        public int PostId { get; set; }
        public int AppUserId { get; set; }
        public Post Post { get; set; }

        public int CommentatorId { get; set; }        
        public AppUser Commentator { get; set; }
    }
}
