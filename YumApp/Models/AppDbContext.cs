using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public class YumAppDbContext : IdentityDbContext<AppUser>
    {
        public YumAppDbContext(DbContextOptions<YumAppDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasKey(p => new { p.Id, p.AppUserId });

            modelBuilder.Entity<Comment>()
                .HasKey(c => new { c.Id, c.PostId });

            modelBuilder.Entity<User_Follows>()
                .HasKey(f => new { f.FollowerId, f.FollowsId });

            modelBuilder.Entity<Post_Ingredient>()
                .HasKey(pi => new { pi.PostId, pi.IngredientId });

            modelBuilder.Entity<UserFeed>()
                .HasOne(uf => uf.AppUser)
                .WithOne(u => u.UserFeed)
                .HasForeignKey<UserFeed>(uf => uf.AppUserId);
            modelBuilder.Entity<UserFeed>()
                .HasKey(uf => uf.AppUserId);
        }
    }
}
