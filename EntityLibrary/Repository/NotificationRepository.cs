using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.Repository
{
    public class NotificationRepository : ICRDRepository<Notification>
    {
        private readonly YumAppDbContext _context;

        public NotificationRepository(YumAppDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> Add(Notification instance)
        {
            if (instance != null)
            {
                await _context.Notifications.AddAsync(instance);
                await _context.SaveChangesAsync();

                return instance;
            }

            return null;
        }

        public IQueryable<Notification> GetAll()
        {
            return _context.Notifications;
        }

        public async Task Remove(Notification instance)
        {
            if (instance != null)
            {
                _context.Notifications.Remove(instance);
                await _context.SaveChangesAsync();
            }
        }
    }
}
