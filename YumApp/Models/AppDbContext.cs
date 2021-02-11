using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public class YumAppDbContext : DbContext
    {
        public YumAppDbContext(DbContextOptions<YumAppDbContext> options) : base(options)
        {
        }

    }
}
