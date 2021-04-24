using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YumApp.Controllers.HelperAndExtensionMethods;
using YumApp.Models;

namespace YumApp.Controllers
{
    public static class UserManagerExtensionMethods
    {
        public static async Task<int> GetCurrentUserIdAsync(this AppUserManager appUserManager, string userEmail)
        {
            AppUser currentUser = await appUserManager.FindByEmailAsync(userEmail);
            int currentUserId = int.Parse(await appUserManager.GetUserIdAsync(currentUser));

            return currentUserId;
        }

        public static async Task<int> GetCurrentUserIdAsync(this AppUserManager appUserManager, ClaimsPrincipal claimsPrincipal)
        {
            AppUser currentUser = await appUserManager.GetUserAsync(claimsPrincipal);
            int currentUserId = int.Parse(await appUserManager.GetUserIdAsync(currentUser));

            return currentUserId;
        }

        public static async Task<AppUserModel> CurrentUserToAppUserModel(this AppUserManager appUserManager, ClaimsPrincipal claimsPrincipal)
        {
            var currentUser = await appUserManager.GetUserAsync(claimsPrincipal);

            return currentUser.ToAppUserModel();
        }

        public static async Task<IdentityResult> UpdateUserAsync(this AppUserManager appUserManager, AppUser model)
        {
            var userToBeUpdated = await appUserManager.FindByIdAsync(model.Id.ToString());

            userToBeUpdated.FirstName = model.FirstName;
            userToBeUpdated.LastName = model.LastName;
            userToBeUpdated.Email = model.Email;
            userToBeUpdated.UserName = model.UserName;
            userToBeUpdated.DateOfBirth = model.DateOfBirth;
            userToBeUpdated.Country = model.Country;
            userToBeUpdated.Gender = model.Gender;
            userToBeUpdated.About = model.About;
            userToBeUpdated.PhotoPath = model.PhotoPath;

            var updatedUser = await appUserManager.UpdateAsync(userToBeUpdated);
            return updatedUser;
        }
    }
}
