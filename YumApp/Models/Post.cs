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
        public Post()
        {
            Comments = new List<Comment>();
            Post_Ingredients = new List<Post_Ingredient>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        
        public List<string> Ingredients { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Minimum lengt is 1 character.")]
        [MaxLength(1000, ErrorMessage = "Maximum lengt is 1000 characters.")]
        public string Content { get; set; }
        [MinLength(1, ErrorMessage = "Minimum lengt is 1 character.")]
        [MaxLength(100, ErrorMessage = "Maximum lengt is 100 characters.")]
        public string Notes { get; set; }
        public DateTime TimeOfPosting { get; set; } = DateTime.Now.ToLocalTime();

        //Navigation properties
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Post_Ingredient> Post_Ingredients { get; set; }

    }
}
