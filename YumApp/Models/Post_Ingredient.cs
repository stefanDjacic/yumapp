using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public class Post_Ingredient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //Navigation properties
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
