using EntityLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp
{
    public class AppDbInitializer
    {
        public static async void Seed(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<YumAppDbContext>();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<int>>>();
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
                var role1 = new IdentityRole<int> { Name = "normaluser" };
                var role2 = new IdentityRole<int> { Name = "admin" };

                await roleManager.CreateAsync(role1);
                await roleManager.CreateAsync(role2);

                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var adminUser = new AppUser()
                {
                    UserName = "admin@gmail.com",
                    FirstName = "admin",
                    LastName = "admin",
                    Email = "admin@gmail.com",
                    DateOfBirth = DateTime.UtcNow,
                    Gender = GenderEnum.Male,
                };

                await userManager.CreateAsync(adminUser, "admin123");

                await userManager.AddToRoleAsync(adminUser, "admin");

                var testUser = new AppUser()
                {
                    UserName = "testuser@gmail.com",
                    FirstName = "test",
                    LastName = "user",
                    Email = "testuser@gmail.com",
                    DateOfBirth = DateTime.UtcNow,
                    Gender = GenderEnum.Male,
                };

                await userManager.CreateAsync(testUser, "testuser123");

                await userManager.AddToRoleAsync(testUser, "normaluser");


                await context.SaveChangesAsync();
            }

            if (!context.Posts.Any())
            {
                var post1 = new Post
                {
                    AppUserId = 2,
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s," +
                    " when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into" +
                    " electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages," +
                    " and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Notes = "Obavezno uzivati",
                };

                var post2 = new Post
                {
                    AppUserId = 2,
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s," +
                    " when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into" +
                    " electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages," +
                    " and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    NumberOfYums = 11
                };

                await context.Posts.AddRangeAsync(new Post[] { post1, post2 });

                await context.SaveChangesAsync();
            }

            if (!context.Comments.Any())
            {
                var comment1 = new Comment
                {
                    AppUserId = 2,
                    PostId = 1,
                    Content = "mmmmmmmmm",
                    CommentatorId = 2
                };

                var comment2 = new Comment
                {
                    AppUserId = 2,
                    PostId = 1,
                    Content = "NICE",
                    CommentatorId = 2
                };

                var comment3 = new Comment
                {
                    AppUserId = 2,
                    PostId = 2,
                    Content = "good job",
                    CommentatorId = 2
                };

                await context.Comments.AddRangeAsync(new Comment[] { comment1, comment2, comment3 });

                await context.SaveChangesAsync();
            }

            if (!context.Ingredients.Any())
            {
                var ingredient1 = new Ingredient
                {
                    Name = "Onion",
                    PhotoPath = @"C:\Users\Korisnik\Desktop\YumApp Photos\Standard ingredient photo",
                    Description = "Very delicios"
                };

                var ingredient2 = new Ingredient
                {
                    Name = "Avocado",
                    PhotoPath = @"C:\Users\Korisnik\Desktop\YumApp Photos\Standard ingredient photo",
                    Description = "Very delicios"
                };

                var ingredient3 = new Ingredient
                {
                    Name = "Chili pepper",
                    PhotoPath = @"C:\Users\Korisnik\Desktop\YumApp Photos\Standard ingredient photo",
                    Description = "Very delicios"
                };

                await context.Ingredients.AddRangeAsync(new Ingredient[] { ingredient1, ingredient2, ingredient3 });
                await context.SaveChangesAsync();
            }

            if (!context.Post_Ingredients.Any())
            {
                var pi1 = new Post_Ingredient
                {
                    PostId = 1,
                    AppUserId = 2,
                    IngredientId = 1                    
                };

                var pi2 = new Post_Ingredient
                {
                    PostId = 1,
                    AppUserId = 2,
                    IngredientId = 2
                };

                var pi3 = new Post_Ingredient
                {
                    PostId = 1,
                    AppUserId = 2,
                    IngredientId = 3
                };

                await context.Post_Ingredients.AddRangeAsync(new Post_Ingredient[] { pi1, pi2, pi3 });

                await context.SaveChangesAsync();
            }
        }
    }
}
