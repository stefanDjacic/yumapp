﻿using EntityLibrary;
using Microsoft.EntityFrameworkCore;
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
                User = new AppUserModel 
                            {
                                Id = pe.AppUser.Id,
                                FirstName = pe.AppUser.FirstName,
                                LastName = pe.AppUser.LastName,
                                Email = pe.AppUser.Email,
                                Username = pe.AppUser.UserName,
                                About = pe.AppUser.About,
                                DateCreated = pe.AppUser.DateCreated,
                                DateOfBirth = pe.AppUser.DateOfBirth,
                                Gender = pe.AppUser.Gender,
                                PhotoPath = pe.AppUser.PhotoPath,
                                Country = pe.AppUser.Country
                            },
                Content = pe.Content,
                Notes = pe.Notes,
                NumberOfYums = pe.NumberOfYums,
                TimeOfPosting = pe.TimeOfPosting,
                Comments = pe.Comments.Select(c => new CommentModel
                                                        {
                                                            Content = c.Content,
                                                            Post = c.Post,
                                                            Commentator = c.Commentator,
                                                            TimeOfCommenting = c.TimeOfCommenting
                                                        }),
                Ingredients = pe.Post_Ingredients.Select(pi => pi.Ingredient).Select(i => new IngredientModel
                                                                                                {
                                                                                                    Name = i.Name,
                                                                                                    Description = i.Description,
                                                                                                    PhotoPath = i.PhotoPath
                                                                                                })
            });
        }

        //public static IQueryable<PostModel> ToPostModel(this IQueryable<Post> entities)
        //{
        //    return entities.Select(pe => new PostModel
        //    {
        //        User = pe.AppUser,
        //        Content = pe.Content,
        //        Notes = pe.Notes,
        //        NumberOfYums = pe.NumberOfYums,
        //        TimeOfPosting = pe.TimeOfPosting,
        //        Comments = pe.Comments.Select(c => new CommentModel
        //                                                {
        //                                                  Content = c.Content,
        //                                                  Post = c.Post,
        //                                                  Commentator = c.Commentator,
        //                                                  TimeOfCommenting = c.TimeOfCommenting
        //                                                }),               
        //        Ingredients = pe.Post_Ingredients.Select(pi => pi.Ingredient)
        //    });
        //}

        //public static IQueryable<PostModel> ToPostModel(this IQueryable<Post> entities)
        //{
        //    return entities.SelectMany(pe => pe.Comments.ToCommentModel(),
        //       (p, c) => new PostModel
        //       {
        //           User = p.AppUser,
        //           Content = p.Content,
        //           Notes = p.Notes,
        //           NumberOfYums = p.NumberOfYums,
        //           TimeOfPosting = p.TimeOfPosting,
        //           Comments = p.Comments.ToCommentModel().Select(c => c),
        //           //Ingredients = pe.Post_Ingredients.AsQueryable().Select(pi => pi.Ingredient).ToIngredientModel().AsEnumerable()
        //           Ingredients = p.Post_Ingredients.Select(pi => pi.Ingredient)
        //       });
        //}
    }

    public class PostModel
    {
        public PostModel()
        {
            Comments = new List<CommentModel>(); 
            Ingredients = new List<IngredientModel>();
        }

        public AppUserModel User { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Minimum lenght is 1 character.")]
        [MaxLength(1000, ErrorMessage = "Maximum lenght is 1000 characters.")]
        public string Content { get; set; }
        
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters.")]
        public string Notes { get; set; }

        public int NumberOfYums { get; set; }

        public DateTime TimeOfPosting { get; set; }

        public IEnumerable<CommentModel> Comments { get; set; }        

        public IEnumerable<IngredientModel> Ingredients { get; set; }
    }
}
