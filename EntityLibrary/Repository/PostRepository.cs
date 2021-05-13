using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public class PostRepository : ICRUDRepository<Post>
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

        public async Task<Post> GetSingle(int id)
        {
            //Find() doesn't work here, because the primary key is composite, so I would need 2 parameters instead of one
            return await _context.Posts.SingleOrDefaultAsync(p => p.Id == id);
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
    }
}
