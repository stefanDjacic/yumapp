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
                    PhotoPath = @"/Photos/DefaultUserPhoto.png"
                };

                await userManager.CreateAsync(adminUser, "admin123");

                await userManager.AddToRoleAsync(adminUser, "admin");

                var testUser1 = new AppUser()
                {
                    UserName = "peraperic@gmail.com",
                    FirstName = "Pera",
                    LastName = "Peric",
                    Email = "peraperic@gmail.com",
                    Country = "Croatia",
                    DateOfBirth = DateTime.UtcNow,
                    Gender = GenderEnum.Male,
                    About = "Very talented young chef.",
                    PhotoPath = @"/Photos/DefaultUserPhoto.png"
                };

                await userManager.CreateAsync(testUser1, "testuser123");

                await userManager.AddToRoleAsync(testUser1, "normaluser");

                var testUser2 = new AppUser()
                {
                    UserName = "maramaric@gmail.com",
                    FirstName = "Mara",
                    LastName = "Maric",
                    Email = "maramaric@gmail.com",
                    Country = "Austria",
                    DateOfBirth = DateTime.UtcNow,
                    Gender = GenderEnum.Female,
                    About = "I like trains!",
                    PhotoPath = @"/Photos/AnotherUserPhoto.jpg"
                };

                await userManager.CreateAsync(testUser2, "testuser123");

                await userManager.AddToRoleAsync(testUser2, "normaluser");

                var testUser3 = new AppUser()
                {
                    UserName = "mikimikic@gmail.com",
                    FirstName = "Miki",
                    LastName = "Mikic",
                    Email = "mikimikic@gmail.com",
                    DateOfBirth = DateTime.UtcNow,
                    Gender = GenderEnum.Male,
                    PhotoPath = @"/Photos/YetAnotherUserPhoto.jpg"
                };

                await userManager.CreateAsync(testUser3, "testuser123");

                await userManager.AddToRoleAsync(testUser3, "normaluser");


                await context.SaveChangesAsync();
            }

            if (!context.Posts.Any())
            {
                var post1 = new Post
                {
                    AppUserId = 2,
                    Content = "Donec ut nibh leo. Ut venenatis sapien sed eros fermentum suscipit. Donec in eleifend sem. Fusce iaculis euismod eros eu placerat. Nunc eu efficitur enim, vel luctus elit. Vivamus posuere nisl risus," +
                    " nec venenatis velit mollis egestas. Donec sagittis in purus sit amet eleifend. Fusce a turpis eu elit ullamcorper hendrerit. Quisque faucibus vestibulum sapien et lobortis.",
                    Notes = "Obavezno uzivati",
                };

                var post2 = new Post
                {
                    AppUserId = 2,
                    Content = "Donec ut nibh leo. Ut venenatis sapien sed eros fermentum suscipit. Donec in eleifend sem. Fusce iaculis euismod eros eu placerat. Nunc eu efficitur enim, vel luctus elit. Vivamus posuere nisl risus," +
                    " nec venenatis velit mollis egestas. Donec sagittis in purus sit amet eleifend. Fusce a turpis eu elit ullamcorper hendrerit. Quisque faucibus vestibulum sapien et lobortis.",
                    NumberOfYums = 11
                };

                var post3 = new Post
                {
                    AppUserId = 3,
                    Content = "Donec ut nibh leo. Ut venenatis sapien sed eros fermentum suscipit. Donec in eleifend sem. Fusce iaculis euismod eros eu placerat. Nunc eu efficitur enim, vel luctus elit. Vivamus posuere nisl risus," +
    " nec venenatis velit mollis egestas. Donec sagittis in purus sit amet eleifend. Fusce a turpis eu elit ullamcorper hendrerit. Quisque faucibus vestibulum sapien et lobortis.",
                    NumberOfYums = 9,
                    Notes = "Happy meal!"
                };

                var post4 = new Post
                {
                    AppUserId = 3,
                    Content = "Donec ut nibh leo. Ut venenatis sapien sed eros fermentum suscipit. Donec in eleifend sem. Fusce iaculis euismod eros eu placerat. Nunc eu efficitur enim, vel luctus elit. Vivamus posuere nisl risus," +
" nec venenatis velit mollis egestas. Donec sagittis in purus sit amet eleifend. Fusce a turpis eu elit ullamcorper hendrerit. Quisque faucibus vestibulum sapien et lobortis.",
                    NumberOfYums = 2,                                   
                };

                var post5 = new Post
                {
                    AppUserId = 4,
                    Content = "Donec ut nibh leo. Ut venenatis sapien sed eros fermentum suscipit. Donec in eleifend sem. Fusce iaculis euismod eros eu placerat. Nunc eu efficitur enim, vel luctus elit. Vivamus posuere nisl risus," +
" nec venenatis velit mollis egestas. Donec sagittis in purus sit amet eleifend. Fusce a turpis eu elit ullamcorper hendrerit. Quisque faucibus vestibulum sapien et lobortis.",
                    NumberOfYums = 99,
                    Notes = "Happy meal!"
                };

                await context.Posts.AddRangeAsync(new Post[] { post1, post2, post3, post4, post5 });

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
                    Content = "Donec ut nibh leo. Ut venenatis sapien sed eros fermentum suscipit. Donec in eleifend sem. Fusce iaculis euismod eros eu placerat. Nunc eu efficitur enim, vel luctus elit. Vivamus posuere nisl risus," +
                    " nec venenatis velit mollis egestas. Donec sagittis in purus sit amet eleifend. Fusce a turpis eu elit ullamcorper hendrerit. Quisque faucibus vestibulum sapien et lobortis.",
                    CommentatorId = 3
                };

                var comment3 = new Comment
                {
                    AppUserId = 2,
                    PostId = 2,
                    Content = "good job",
                    CommentatorId = 4
                };

                var comment4 = new Comment
                {
                    AppUserId = 4,
                    PostId = 5,
                    Content = "yumyum",
                    CommentatorId = 3
                };

                var comment5 = new Comment
                {
                    AppUserId = 4,
                    PostId = 5,
                    Content = "yeayea",
                    CommentatorId = 2
                };

                await context.Comments.AddRangeAsync(new Comment[] { comment1, comment2, comment3, comment4, comment5 });

                await context.SaveChangesAsync();
            }

            if (!context.Ingredients.Any())
            {
                var ingredient1 = new Ingredient
                {
                    Name = "Onion",
                    PhotoPath = @"/Photos/DefaultIngredientPhoto.jpg",
                    Description = "Donec ut nibh leo. Ut venenatis sapien sed eros fermentum suscipit. Donec in eleifend sem. Fusce iaculis euismod eros eu placerat. Nunc eu efficitur enim, vel luctus elit."
                };

                var ingredient2 = new Ingredient
                {
                    Name = "Avocado",
                    PhotoPath = @"/Photos/DefaultIngredientPhoto.jpg",
                    Description = "Donec ut nibh leo. Ut venenatis sapien sed eros fermentum suscipit. Donec in eleifend sem. Fusce iaculis euismod eros eu placerat. Nunc eu efficitur enim, vel luctus elit."
                };

                var ingredient3 = new Ingredient
                {
                    Name = "Chili pepper",
                    PhotoPath = @"/Photos/DefaultIngredientPhoto.jpg",
                    Description = "Donec ut nibh leo. Ut venenatis sapien sed eros fermentum suscipit. Donec in eleifend sem. Fusce iaculis euismod eros eu placerat. Nunc eu efficitur enim, vel luctus elit."
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
