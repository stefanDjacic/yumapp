using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public class CommentRepository : ICRDRepository<Comment>
    {
        private readonly YumAppDbContext _context;

        public CommentRepository(YumAppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> Add(Comment instance)
        {
            if (instance != null)
            {
                await _context.Comments.AddAsync(instance);
                await _context.SaveChangesAsync();

                return instance;
            }

            return null;
        }

        public IQueryable<Comment> GetAll()
        {
            return _context.Comments;
        }

        public async Task Remove(Comment instance)
        {
            if (instance != null)
            {
                _context.Comments.Remove(instance);
                await _context.SaveChangesAsync();
            }
        }
    }
}
