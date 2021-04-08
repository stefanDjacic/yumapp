using EntityLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public static class CommentModelExtensionMethods
    {
        public static IQueryable<CommentModel> ToCommentModel(this IQueryable<Comment> entities)
        {
            return entities.Select(ce => new CommentModel
            {
                Content = ce.Content,
                TimeOfCommenting = ce.TimeOfCommenting,
                Post = ce.Post,
                Commentator = new AppUserModel
                {
                    Id = ce.Commentator.Id,
                    FirstName = ce.Commentator.FirstName,
                    LastName = ce.Commentator.LastName,
                    Email = ce.Commentator.Email,
                    Username = ce.Commentator.UserName,
                    DateOfBirth = ce.Commentator.DateOfBirth,
                    Country = ce.Commentator.Country,
                    Gender = ce.Commentator.Gender,
                    About = ce.Commentator.About,
                    PhotoPath = ce.Commentator.PhotoPath
                }
            });
        }

        public static IEnumerable<CommentModel> ToCommentModel(this IEnumerable<Comment> entities)
        {
            return entities.Select(ce => new CommentModel
            {
                Content = ce.Content,
                TimeOfCommenting = ce.TimeOfCommenting,
                Post = ce.Post,
                Commentator = new AppUserModel
                {
                    Id = ce.Commentator.Id,
                    FirstName = ce.Commentator.FirstName,
                    LastName = ce.Commentator.LastName,
                    Email = ce.Commentator.Email,
                    Username = ce.Commentator.UserName,
                    DateOfBirth = ce.Commentator.DateOfBirth,
                    Country = ce.Commentator.Country,
                    Gender = ce.Commentator.Gender,
                    About = ce.Commentator.About,
                    PhotoPath = ce.Commentator.PhotoPath
                }
            });
        }
    }

    public class CommentModel
    {
        [Required]
        [MinLength(1, ErrorMessage = "Minimum lenght is 1 character.")]
        [MaxLength(500, ErrorMessage = "Maximum lenght is 500 characters.")]
        public string Content { get; set; }
        public DateTime TimeOfCommenting { get; set; }
        public Post Post { get; set; }
        public AppUserModel Commentator { get; set; }
    }
}
