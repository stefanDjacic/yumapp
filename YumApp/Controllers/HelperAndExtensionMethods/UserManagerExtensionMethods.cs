using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YumApp.Models;

namespace YumApp.Controllers
{
    public static class UserManagerExtensionMethods
    {
        public static async Task<int> GetCurrentUserIdAsync(this UserManager<AppUser> userManager, string userEmail)
        {
            AppUser currentUser = await userManager.FindByEmailAsync(userEmail);
            int currentUserId = int.Parse(await userManager.GetUserIdAsync(currentUser));

            return currentUserId;
        }

        public static async Task<int> GetCurrentUserIdAsync(this UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal)
        {
            AppUser currentUser = await userManager.GetUserAsync(claimsPrincipal);
            int currentUserId = int.Parse(await userManager.GetUserIdAsync(currentUser));

            return currentUserId;
        }

        public static async Task<AppUserModel> CurrentUserToAppUserModel(this UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal)
        {
            var currentUser = await userManager.GetUserAsync(claimsPrincipal);

            return currentUser.ToAppUserModel();
        }

        public static async Task<IdentityResult> UpdateUserAsync(this UserManager<AppUser> userManager, AppUser model)
        {
            var userToBeUpdated = await userManager.FindByIdAsync(model.Id.ToString());

            userToBeUpdated.FirstName = model.FirstName;
            userToBeUpdated.LastName = model.LastName;
            userToBeUpdated.Email = model.Email;
            userToBeUpdated.UserName = model.UserName;
            userToBeUpdated.DateOfBirth = model.DateOfBirth;
            userToBeUpdated.Country = model.Country;
            userToBeUpdated.Gender = model.Gender;
            userToBeUpdated.About = model.About;
            userToBeUpdated.PhotoPath = model.PhotoPath;

            var result = await userManager.UpdateAsync(userToBeUpdated);
            return result;
        }
    }
}
