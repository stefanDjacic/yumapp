using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class YumAppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
    {
        public YumAppDbContext(DbContextOptions<YumAppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Post_Ingredient> Post_Ingredients { get; set; }
        public DbSet<User_Follows> User_Follows { get; set; }
        public DbSet<User_Feed> User_Feeds { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
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

                c.HasOne(c => c.Commentator)
                .WithMany(au => au.Comments)
                .HasForeignKey(c => c.CommentatorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

                c.HasKey(c => new { c.Id, c.PostId, c.AppUserId });
            });

            modelBuilder.Entity<User_Follows>(uf =>
            {
                uf.HasOne(uf => uf.Follower)
                .WithMany(au => au.Followers)
                .HasForeignKey(uf => uf.FollowerId)                
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

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
                .HasForeignKey(pi => new { pi.PostId, pi.AppUserId })
                .IsRequired();

                pi.HasOne(pi => pi.Ingredient)
                .WithMany(i => i.Post_Ingredients)
                .HasForeignKey(pi => pi.IngredientId)
                .IsRequired();

                pi.HasKey(pi => new { pi.PostId, pi.AppUserId, pi.IngredientId });
            });


            modelBuilder.Entity<User_Feed>(uf =>
            {
                uf.HasOne(uf => uf.AppUser)
                .WithMany(au => au.User_Feeds)
                .HasForeignKey(uf => uf.AppUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

                uf.HasOne(uf => uf.Post)
                .WithMany(p => p.User_Feeds)
                .HasForeignKey(uf => new { uf.PostId, uf.PostAppUserId })
                .IsRequired();

                uf.HasKey(uf => new { uf.AppUserId, uf.PostId, uf.PostAppUserId });
            });

            modelBuilder.Entity<AppUser>(au =>
            {
                au.Property(au => au.Gender)
                .HasConversion<int>();

                au.Property(p =>p.Email).IsRequired();
                au.Property(p => p.PasswordHash).IsRequired();
                au.Property(p => p.UserName).IsRequired();
            });                        
        }
    }
}
