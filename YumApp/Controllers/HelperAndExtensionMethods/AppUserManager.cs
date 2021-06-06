using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using YumApp.Models;

namespace YumApp.Controllers.HelperAndExtensionMethods
{
    //Didn't need in the end
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
        public AppUserStore MyStore => Store as AppUserStore;
    }
}
