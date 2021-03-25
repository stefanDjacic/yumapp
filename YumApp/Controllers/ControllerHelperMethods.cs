using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YumApp.Controllers
{
    public static class ControllerHelperMethods
    {
        public static async Task<int> GetCurrentUserIdAsync(UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal)
        {
            var currentUser = await userManager.GetUserAsync(claimsPrincipal);
            var output = int.Parse(await userManager.GetUserIdAsync(currentUser));

            return output;
        }
    }
}
