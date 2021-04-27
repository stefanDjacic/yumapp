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
        public static IQueryable<CommentModel> ToCommentModel(this IQueryable<Comment> comments)
        {
            return comments.Select(c => new CommentModel
            {
                Content = c.Content,
                TimeOfCommenting = c.TimeOfCommenting,
                //Post = c.Post,
                Commentator = c.Commentator.ToAppUserModelBaseInfo()
            });
        }

        public static IEnumerable<CommentModel> ToCommentModel(this IEnumerable<Comment> comments)
        {
            return comments.Select(c => new CommentModel
            {
                Content = c.Content,
                TimeOfCommenting = c.TimeOfCommenting,
                //Post = c.Post,
                Commentator = c.Commentator.ToAppUserModelBaseInfo()
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
        //public Post Post { get; set; }
        public AppUserModel Commentator { get; set; }
    }
}
