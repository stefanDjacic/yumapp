using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public class Yummy_PostsRepository : ICRDRepository<Yummy_Post>
    {
        private readonly YumAppDbContext _context;

        public Yummy_PostsRepository(YumAppDbContext context)
        {
            _context = context;
        }

        public async Task<Yummy_Post> Add(Yummy_Post instance)
        {
            if (instance != null)
            {
                await _context.Yummy_Posts.AddAsync(instance);
                await _context.SaveChangesAsync();

                return instance;
            }

            return null;
        }

        public IQueryable<Yummy_Post> GetAll()
        {
            return _context.Yummy_Posts;
        }

        public async Task Remove(Yummy_Post instance)
        {
            if (instance != null)
            {
                _context.Yummy_Posts.Remove(instance);
                await _context.SaveChangesAsync();
            }
        }
    }
}
