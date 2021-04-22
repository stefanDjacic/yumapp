using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YumApp.Models;

namespace YumApp.Controllers.HelperAndExtensionMethods
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(AppUserStore store,
                              IOptions<IdentityOptions> optionsAccessor,
                              IPasswordHasher<AppUser> passwordHasher,
                              IEnumerable<IUserValidator<AppUser>> userValidators,
                              IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
                              ILookupNormalizer keyNormalizer,
                              IdentityErrorDescriber errors,
                              IServiceProvider services,
                              ILogger<UserManager<AppUser>> logger)
                              : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        //Hiding Store property of parent class because of needed type.
        //Don't know if it's bad practice, I could just cast it every time I use it.
        public new AppUserStore Store { get; set; }
        public AppUserModel GetUserWithNotifications(ClaimsPrincipal claimsPrincipal)
        {
            return Store.GetUserWithNotifications(claimsPrincipal);
        }        
    }

    public class AppUserStore : UserStore<AppUser, IdentityRole<int>, YumAppDbContext, int>
    {
        public AppUserStore(YumAppDbContext context, IdentityErrorDescriber identityErrorDescriber = null) : base(context, identityErrorDescriber)
        {
        }

        public AppUserModel GetUserWithNotifications(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                return null;
            }

            var appUserModelWithNotifications = Context.AppUsers.Where(au => au.Id == int.Parse(claimsPrincipal.Identity.Name))
                                                        .Include(au => au.NotificationsReceiver)
                                                        /*.Include(au => au.NotificationsInitiator)*/           //need this for photo
                                                        .SingleOrDefault()
                                                        .ToAppUserModel();

            return appUserModelWithNotifications;
        }
    }
}
