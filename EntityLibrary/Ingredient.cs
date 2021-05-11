using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class Ingredient
    {
        public Ingredient()
        {
            Post_Ingredients = new HashSet<Post_Ingredient>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Minimum lengt is 2 characters.")]
        [MaxLength(50, ErrorMessage = "Maximum lengt is 50 characters.")]
        public string Name { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Minimum lengt is 2 characters.")]
        [MaxLength(500, ErrorMessage = "Maximum lengt is 50 characters.")]
        public string Description { get; set; }

        [Required]
        public string PhotoPath { get; set; }

        //Navigation properties
        public ICollection<Post_Ingredient> Post_Ingredients { get; set; }
    }
}
