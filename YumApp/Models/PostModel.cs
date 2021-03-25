using EntityLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public static class PostModelExtensionMethods
    {
        public static IQueryable<PostModel> ToPostModel(this IQueryable<Post> entities)
        {
            return entities.Select(pe => new PostModel
            {
                Content = pe.Content,
                Notes = pe.Notes,
                TimeOfPosting = pe.TimeOfPosting,
                Comments = pe.Comments,
                //Post_Ingredients = pe.Post_Ingredients,
                Ingredients = pe.Post_Ingredients.Select(pi => pi.Ingredient)
            });
        }
    }

    public class PostModel
    {
        public PostModel()
        {
            Comments = new List<Comment>();
            //Post_Ingredients = new HashSet<Post_Ingredient>();
            Ingredients = new List<Ingredient>();
        }

        [Required]
        [MinLength(1, ErrorMessage = "Minimum lengt is 1 character.")]
        [MaxLength(1000, ErrorMessage = "Maximum lengt is 1000 characters.")]
        public string Content { get; set; }
        
        [MaxLength(100, ErrorMessage = "Maximum lengt is 100 characters.")]
        public string Notes { get; set; }

        public DateTime TimeOfPosting { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        //public ICollection<Post_Ingredient> Post_Ingredients { get; set; }

        public IEnumerable<Ingredient> Ingredients { get; set; }
    }
}
