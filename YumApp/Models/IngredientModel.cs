﻿using EntityLibrary;
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
        public static IngredientModel ToIngredientModel(this Ingredient ingredient)
        {
            return new IngredientModel
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Description = ingredient.Description,
                PhotoPath = ingredient.PhotoPath
            };
        }

        public static IEnumerable<IngredientModel> ToIngredientModel(this IEnumerable<Ingredient> ingredients)
        {
            return ingredients.Select(i => new IngredientModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                PhotoPath = i.PhotoPath
            });
        }

        public static IQueryable<IngredientModel> ToIngredientModel(this IQueryable<Ingredient> ingredients)
        {
            return ingredients.Select(i => new IngredientModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                PhotoPath = i.PhotoPath
            });
        }

        public static Ingredient ToIngredientEntity(this IngredientModel ingredientModel)
        {
            return new Ingredient
            {
                Name = ingredientModel.Name,
                Description = ingredientModel.Description,
                PhotoPath = ingredientModel.PhotoPath
            };
        }
    }


    public class IngredientModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Minimum lengt is 2 characters.")]
        [MaxLength(50, ErrorMessage = "Maximum lengt is 50 characters.")]
        public string Name { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Minimum lengt is 2 characters.")]
        [MaxLength(500, ErrorMessage = "Maximum lengt is 50 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Photo is required.")]
        public string PhotoPath { get; set; }
        
        //Other
        public IFormFile Photo { get; set; }
    }
}
