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

        public static IEnumerable<CommentModel> ToCommentModel(this IEnumerable<Comment> comments)
        {
            return comments.Select(c => new CommentModel
            {
                Content = c.Content,
                TimeOfCommenting = c.TimeOfCommenting,                
                Commentator = c.Commentator.ToAppUserModelBaseInfo()
            });
        }

        public static Comment ToCommentEntity(this CommentModel commentsModel)
        {
            return new Comment
            {
                PostId = commentsModel.PostId,
                AppUserId = commentsModel.AppUserId,
                CommentatorId = commentsModel.Commentator.Id,
                Content = commentsModel.Content,
                TimeOfCommenting = commentsModel.TimeOfCommenting,                
            };
        }
    }

    public class CommentModel
    {
        public int PostId { get; set; }
        public int AppUserId { get; set; }
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
