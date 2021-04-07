using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public static class TestHelperExtensions
    {
        public static IQueryable<PostTest> ToPostTest(this IQueryable<Post> posts)
        {
            return posts.Select(p => new PostTest
            {
                Id = p.Id,
                Content = p.Content,
                Name = p.AppUser.FirstName,
                Comments = p.Comments.ToTestComments().ToList()
            });
        }

        public static IQueryable<PostTest> ToPostTest1(this IQueryable<Post> posts)
        {
            return posts.Select(p => new PostTest
            {
                Id = p.Id,
                Content = p.Content,
                Name = p.AppUser.FirstName,
                Comments = p.Comments.Select(c => new TestComment
                {
                    Content = c.Content,
                    TestAppUser = new TestAppUser
                    {
                        Id = c.Commentator.Id,
                        Name = c.Commentator.FirstName
                    }
                })
            });
        }

        public static IQueryable<TestComment> ToTestComments(this IQueryable<Comment> comments)
        {
            return comments.Select(c => new TestComment
            {
                Content = c.Content,
                TestAppUser = c.Commentator.ToTestAppUser()
            });
        }

        public static IEnumerable<TestComment> ToTestComments(this IEnumerable<Comment> comments)
        {
            return comments.Select(c => new TestComment
            {
                Content = c.Content,
                TestAppUser = c.Commentator.ToTestAppUser()
            });
        }

        public static TestAppUser ToTestAppUser(this AppUser appUser)
        {
            return new TestAppUser
            {
                Id = appUser.Id,
                Name = appUser.FirstName
            };
        }
    }

    public class PostTest
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public IEnumerable<TestComment> Comments { get; set; }
    }

    public class TestComment
    {
        public string Content { get; set; }
        public TestAppUser TestAppUser { get; set; }
    }

    public class TestAppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
