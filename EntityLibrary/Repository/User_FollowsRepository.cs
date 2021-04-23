using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public class User_FollowsRepository : ICRDRepository<User_Follows>
    {
        private readonly YumAppDbContext _context;

        public User_FollowsRepository(YumAppDbContext context)
        {
            _context = context;
        }

        public async Task<User_Follows> Add(User_Follows instance)
        {
            if (instance != null)
            {
                await _context.User_Follows.AddAsync(instance);
                await _context.SaveChangesAsync();

                return instance;
            }

            return null;
        }

        public IQueryable<User_Follows> GetAll()
        {
            return _context.User_Follows;
        }

        public async Task Remove(User_Follows instance)
        {
            if (instance != null)
            {
                _context.User_Follows.Remove(instance);
                await _context.SaveChangesAsync();
            }
        }

        //public IQueryable<User_Follows> GetAll()
        //{
        //    return _context.User_Follows.Include(uf => uf.Follower).Include(uf => uf.Follows);
        //}
    }
}
