using EntityLibrary;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public static class IngredientModelExtensionMethods
    {
        public static IQueryable<IngredientModel> ToIngredientModel(this IQueryable<Ingredient> entities)
        {
            return entities.Select(i => new IngredientModel
            {
                Name = i.Name,
                Description = i.Description,
                PhotoPath = i.PhotoPath
            });
        }
    }


    public class IngredientModel
    {
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

        //public ICollection<Post_Ingredient> Post_Ingredients { get; set; }

        public IFormFile Photo { get; set; }
    }
}
