using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Post_Ingredients = new HashSet<Post_Ingredient>();
            User_Feeds = new HashSet<User_Feed>();
            YummyPosts = new HashSet<YummyPost>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        

        [Required]
        [MinLength(1, ErrorMessage = "Minimum lengt is 1 character.")]
        [MaxLength(1000, ErrorMessage = "Maximum lengt is 1000 characters.")]
        public string Content { get; set; }
        
        [MaxLength(100, ErrorMessage = "Maximum lengt is 100 characters.")]
        public string Notes { get; set; }

        public int NumberOfYums { get; set; } = 0;

        public DateTime TimeOfPosting { get; set; } /*= DateTime.Now.ToLocalTime();*/

        //Navigation properties
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<Comment> Comments { get; set; }        

        public ICollection<Post_Ingredient> Post_Ingredients { get; set; }

        public ICollection<User_Feed> User_Feeds { get; set; }

        public ICollection<YummyPost> YummyPosts { get; set; }

    }
}
