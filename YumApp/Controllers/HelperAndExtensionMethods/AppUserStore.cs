using EntityLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YumApp.Models;

namespace YumApp.Controllers.HelperAndExtensionMethods
{
    //Didn't need in the end
    public class AppUserStore : UserStore<AppUser, IdentityRole<int>, YumAppDbContext, int>
    {
        public AppUserStore(YumAppDbContext context,
                            IdentityErrorDescriber identityErrorDescriber = null)
                            : base(context, identityErrorDescriber)
        {
        }
    }
}

