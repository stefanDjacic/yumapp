using EntityLibrary;
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
        #region Most efficient query for postmodel, use it when entity framework core 6.0 gets released!!
        //Most efficient query for postmodel, use it when entity framework core 6.0 gets released!! Don't forget to chain AsSplitQuery()!!
        //public static IQueryable<PostModel> ToPostModelTest(this IQueryable<Post> posts)
        //{
        //    return posts.Select(p => new PostModel
        //    {
        //        User = new AppUserModel {
        //            Id = p.AppUser.Id,
        //            FirstName = p.AppUser.FirstName,
        //            LastName = p.AppUser.LastName,
        //            Email = p.AppUser.Email,
        //            Username = p.AppUser.UserName,
        //            DateOfBirth = p.AppUser.DateOfBirth,
        //            Country = p.AppUser.Country,
        //            Gender = p.AppUser.Gender,
        //            About = p.AppUser.About,
        //            PhotoPath = p.AppUser.PhotoPath
        //        },
        //        Content = p.Content,
        //        Notes = p.Notes,
        //        NumberOfYums = p.NumberOfYums,
        //        TimeOfPosting = p.TimeOfPosting,
        //        Comments = p.Comments.AsQueryable().Select(CommentModelExtensionMethods.MapCommentToCommentModelTest).ToList(),
        //        Ingredients = p.Post_Ingredients.Select(pi => pi.Ingredient).ToIngredientModel().ToList()
        //    });
        //}        
        #endregion

        public static PostModel ToPostModel(this Post post)
        {
            return new PostModel
            {
                Id = post.Id,
                Comments = post.Comments.ToCommentModel().ToList(),
                Content = post.Content,
                Ingredients = post.Post_Ingredients.Select(pi => pi.Ingredient).ToIngredientModel().ToList(),
                Notes = post.Notes,
                NumberOfYums = post.NumberOfYums,
                TimeOfPosting = post.TimeOfPosting,
                User = post.AppUser.ToAppUserModelBaseInfo()
            };
        }

        public static IEnumerable<PostModel> ToPostModel(this IEnumerable<Post> posts)
        {
            return posts.Select(p => new PostModel
            {
                Id = p.Id,
                User = p.AppUser.ToAppUserModelBaseInfo(),
                Content = p.Content,
                Notes = p.Notes,
                NumberOfYums = p.NumberOfYums,
                TimeOfPosting = p.TimeOfPosting,
                Comments = p.Comments.ToCommentModel().ToList(),
                Ingredients = p.Post_Ingredients.Select(pi => pi.Ingredient).ToIngredientModel().ToList()
            });
        }

        public static Post ToPostEntity(this PostModel postModel)
        {
            return new Post
            {
                AppUserId = postModel.User.Id,
                Content = postModel.Content,
                Notes = postModel.Notes,
                NumberOfYums = postModel.NumberOfYums,
                TimeOfPosting = postModel.TimeOfPosting
            };
        }
    }

    public class PostModel
    {
        public PostModel()
        {
            Comments = new List<CommentModel>(); 
            Ingredients = new List<IngredientModel>();
        }

        public int Id { get; set; }

        public AppUserModel User { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Minimum lenght is 1 character.")]
        [MaxLength(1000, ErrorMessage = "Maximum lenght is 1000 characters.")]
        public string Content { get; set; }
        
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters.")]
        public string Notes { get; set; }

        public int NumberOfYums { get; set; }

        public DateTime TimeOfPosting { get; set; }

        public bool IsPostYummed { get; set; }

        public IEnumerable<CommentModel> Comments { get; set; }        

        public IEnumerable<IngredientModel> Ingredients { get; set; }
    }
}
