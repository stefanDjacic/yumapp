using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class Post_Ingredient
    {
        //Navigation properties
        public int PostId { get; set; }
        public int AppUserId { get; set; }        
        public Post Post { get; set; }
        public int IngredientId { get; set; }        
        public Ingredient Ingredient { get; set; }

    }
}
