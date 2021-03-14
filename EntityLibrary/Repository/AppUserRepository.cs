using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public class AppUserRepository : ICRUDRepository<AppUser>
    {
        private readonly YumAppDbContext _context;

        public AppUserRepository(YumAppDbContext context)
        {
            _context = context;
        }

        public AppUser Add(AppUser entry)
        {
            if (entry != null)
            {
                _context.Add(entry);
                _context.SaveChangesAsync();

                return entry;
            }

            return entry;
        }

        public IQueryable<AppUser> GetAll()
        {
            return _context.AppUsers;
        }

        public AppUser GetSingle(int id)
        {
            AppUser user = _context.AppUsers.Find(id);

            if (user != null)            
                return user;

            return null;
        }

        public void Remove(AppUser entry)
        {
            AppUser user = _context.AppUsers.Find(entry.Id);

            if (user != null)
            {
                _context.AppUsers.Remove(user);
                _context.SaveChanges();
            }
        }

        public AppUser Update(AppUser entry)
        {
            if (entry != null)
            {
                _context.AppUsers.Update(entry);
                _context.SaveChanges();

                return entry;
            }

            return entry;
        }
    }
}
