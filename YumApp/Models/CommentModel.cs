using EntityLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public static class CommentModelExtensionMethods
    {
        #region Most efficient query for commentModel, use it when entity framework core 6.0 gets released!!
        //public static readonly Expression<Func<Comment, CommentModel>> MapCommentToCommentModelTest =
        //    comment => new CommentModel
        //    {
        //        Content = comment.Content,
        //        TimeOfCommenting = comment.TimeOfCommenting,
        //        CommentatorId = comment.Commentator.Id,
        //        CommentatorFirstName = comment.Commentator.FirstName,
        //        CommentatorLastName = comment.Commentator.LastName,
        //        CommentatorPhotoPath = comment.Commentator.PhotoPath
        //    };

        //public static IQueryable<CommentModel> ToCommentModelTest(this IQueryable<Comment> comments)
        //{
        //    return comments.Select(MapCommentToCommentModelTest);
        //}

        //public static IEnumerable<CommentModel> ToCommentModelTest(this IEnumerable<Comment> comments)
        //{
        //    return comments.Select(c => new CommentModel
        //    {
        //        Content = c.Content,
        //        TimeOfCommenting = c.TimeOfCommenting,
        //        CommentatorId = c.Commentator.Id,
        //        CommentatorFirstName = c.Commentator.FirstName,
        //        CommentatorLastName = c.Commentator.LastName,
        //        CommentatorPhotoPath = c.Commentator.PhotoPath
        //    });
        //}
        #endregion

        //public static IQueryable<CommentModel> ToCommentModel(this IQueryable<Comment> comments)
        //{
        //    return comments.Select(c => new CommentModel
        //    {
        //        Content = c.Content,
        //        TimeOfCommenting = c.TimeOfCommenting,                
        //        Commentator = c.Commentator.ToAppUserModelBaseInfo()
        //    });
        //}

        public static IEnumerable<CommentModel> ToCommentModel(this IEnumerable<Comment> comments)
        {
            return comments.Select(c => new CommentModel
            {
                Content = c.Content,
                TimeOfCommenting = c.TimeOfCommenting,                
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
        public AppUserModel Commentator { get; set; }

        #region use these when entity framework core 6.0 gets released!!
        //use these when entity framework core 6.0 gets released!!
        //public int CommentatorId { get; set; }
        //public string CommentatorFirstName { get; set; }
        //public string CommentatorLastName { get; set; }
        //public string CommentatorPhotoPath { get; set; }
        #endregion
    }
}
