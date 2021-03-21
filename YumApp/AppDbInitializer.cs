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
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<YumAppDbContext>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<int>>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();

                if (!context.Roles.Any())
                {
                    var role1 = new IdentityRole<int> { Name = "normalUser" };
                    var role2 = new IdentityRole<int> { Name = "admin" };

                    await roleManager.CreateAsync(role1);
                    await roleManager.CreateAsync(role2);

                    await context.SaveChangesAsync();
                }

                if (!context.Users.Any())
                {
                    var adminUser = new AppUser()
                    {
                        FirstName = "admin",
                        LastName = "admin",
                        Email = "admin@gmail.com",
                        DateOfBirth = DateTime.UtcNow,
                        Gender = GenderEnum.Male,                        
                    };

                    await userManager.CreateAsync(adminUser, "admin123");

                    await userManager.AddToRoleAsync(adminUser, "admin");

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
