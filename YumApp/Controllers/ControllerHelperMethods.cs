using EntityLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using YumApp.Models;

namespace YumApp.Controllers
{
    public static class ControllerHelperMethods
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

        public static async Task<HttpResponseMessage> CallApi(HttpMethod httpMethod, string uri, IHttpClientFactory httpClientFactory)
        {
            try
            {
                var request = new HttpRequestMessage(httpMethod, uri);
                var client = httpClientFactory.CreateClient();
                HttpResponseMessage responseMessage = await client.SendAsync(request);

                return responseMessage;
            }
            catch
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

                return responseMessage;
            }

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

        //public static ValidationRespone CheckModelState(AppUserModel model, ModelStateDictionary modelState)
        //{
        //    if (!modelState.IsValid)
        //    {
        //        return View(model);
        //    }            
        //}
    }
}
