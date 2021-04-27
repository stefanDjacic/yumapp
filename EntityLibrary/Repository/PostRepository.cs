using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public class PostRepository : ICRUDRepository<Post>  /*IPostRepository */
    {
        private readonly YumAppDbContext _context;

        public PostRepository(YumAppDbContext context)
        {
            _context = context;
        }

        public async Task<Post> Add(Post instance)
        {
            if (instance != null)
            {
                await _context.Posts.AddAsync(instance);
                await _context.SaveChangesAsync();

                return instance;
            }

            return null;
        }

        public IQueryable<Post> GetAll()
        {
            return _context.Posts;
        }

        //public IQueryable<Post> GetAllWhere()
        //{
        //    var posts = _context.Posts/*.Where(p => p.AppUserId == id)*/;
        //    posts.Include(p => p.Post_Ingredients).ThenInclude(pi => pi.Ingredient).Load();
        //    posts.Include(p => p.AppUser).Load();
        //    posts.Include(p => p.Comments).ThenInclude(c => c.Commentator).Load();


        //    return posts;
        //}

        public async Task<Post> GetSingle(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task Remove(Post instance)
        {
            if (instance != null)
            {
                _context.Posts.Remove(instance);
                await _context.SaveChangesAsync();
            } 
        }

        public async Task<Post> Update(Post instance)
        {
            if (instance != null)
            {
                _context.Update(instance);
                await _context.SaveChangesAsync();

                return instance;
            }

            return null;
        }

        //Additional methods
        //public IQueryable<Ingredient> GetIngredienstFromPost(int postId)
        //{
        //    return (IQueryable<Ingredient>)_context.Posts
        //                    .Include(p => p.Post_Ingredients
        //                                .Where(pi => pi.PostId == postId))
        //                    .ThenInclude(pi => pi.Ingredient)
        //                    /*.Select(i => new Ingredient())*/;
        //}

    }
}
