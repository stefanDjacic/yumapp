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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasKey(p => new { p.Id, p.AppUserId });

            modelBuilder.Entity<Comment>(c => {
                c.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => new { c.PostId, c.AppUserId })
                .IsRequired();

                c.HasKey(c => new { c.Id, c.PostId, c.AppUserId });
            });

            modelBuilder.Entity<User_Follows>(uf =>
            {
                uf.HasOne(uf => uf.Follower)
                .WithMany(au => au.Followers)
                .HasForeignKey(uf => uf.FollowerId)
                .IsRequired();

                uf.HasOne(uf => uf.Follows)
                .WithMany(au => au.Follow)
                .HasForeignKey(uf => uf.FollowsId)
                .IsRequired();

                uf.HasKey(uf => new { uf.FollowerId, uf.FollowsId });
            });

            modelBuilder.Entity<Post_Ingredient>(pi => 
            {
                pi.HasOne(pi => pi.Post)
                .WithMany(p => p.Post_Ingredients)
                .HasForeignKey(pi => pi.PostId)
                .IsRequired();

                pi.HasOne(pi => pi.Ingredient)
                .WithMany(i => i.Post_Ingredients)
                .HasForeignKey(pi => pi.IngredientId)
                .IsRequired();

                pi.HasKey(pi => new { pi.PostId, pi.IngredientId });
            });


            modelBuilder.Entity<UserFeed>(uf => 
            {
                uf.HasOne(uf => uf.AppUser)
                .WithMany(au => au.UserFeeds)
                .HasForeignKey(uf => uf.AppUserId)
                .IsRequired();

                uf.HasOne(uf => uf.Post)
                .WithMany(p => p.UserFeeds)
                .HasForeignKey(uf => uf.PostId)
                .IsRequired();

                uf.HasKey(uf => new { uf.AppUserId, uf.PostId });
            });
        }
    }
}
