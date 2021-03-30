using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace YumApp.Controllers
{
    public static class ControllerHelperMethods
    {
        public static async Task<int> GetCurrentUserIdAsync(UserManager<AppUser> userManager, string userEmail)
        {
            AppUser currentUser = await userManager.FindByEmailAsync(userEmail);
            int output = int.Parse(await userManager.GetUserIdAsync(currentUser));

            return output;
        }

        public static async Task<int> GetCurrentUserIdAsync(UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal)
        {
            AppUser currentUser = await userManager.GetUserAsync(claimsPrincipal);
            int output = int.Parse(await userManager.GetUserIdAsync(currentUser));

            return output;
        }
    }
}
