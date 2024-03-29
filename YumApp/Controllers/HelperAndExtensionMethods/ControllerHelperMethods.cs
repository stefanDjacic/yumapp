﻿using EntityLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
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
        #region Cookie Methods
        public static void CreateUserIdCookie(this Controller controller, int userId, string path, string name)
        {
            CookieOptions cookieOptions = new ();
            cookieOptions.Path = path;
            cookieOptions.Expires = DateTime.Now.AddDays(5);
            controller.Response.Cookies.Append(name, userId.ToString(), cookieOptions);
        }

        public static void RemoveUserIdCookie(this Controller controller)
        {
            controller.Response.Cookies.Delete("MyCookie");
        }

        public static int GetCurrentUserIdFromCookie(this Controller controller)
        {
            int currentUserId = int.Parse(controller.HttpContext.Request.Cookies["MyCookie"]);

            return currentUserId;
        }
        #endregion


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

        #region Photo Methods
        public static async Task SavePhoto(IFormFile photo, string folderPath, int id)
        {
            if (photo == null)
            {
                return;
            }

            if (photo.Length == 0)
            {
                return; 
            }

            var photoName = photo.FileName.Insert(0, id.ToString() + "_");
            var fullPath = Path.Combine(folderPath + photoName);

            //Deletes old photo
            DeletePhoto(id, fullPath);

            //Creates updated photo
            using var stream = File.Create(fullPath);
            await photo.CopyToAsync(stream);

            return;
        }

        public static async Task SaveIngredientPhoto(IFormFile photo, string folderPath)
        {
            if (photo == null)
            {
                return;
            }

            if (photo.Length == 0)
            {
                return;
            }
            
            var fullPath = Path.Combine(folderPath + photo.FileName);

            //Creates updated photo
            using var stream = File.Create(fullPath);
            await photo.CopyToAsync(stream);

            return;
        }

        private static void DeletePhoto(int id, string filePath)
        {            
            //Gets user photo path with the same id value as user's who is updating profile photo if it exists
            var photoPathWithSameIdAsUser = Directory.GetFiles(filePath.Substring(0, filePath.LastIndexOf('\\')))
                                                 .SingleOrDefault(f => f.Substring(filePath.LastIndexOf('\\') + 1, id.ToString().Length) == id.ToString());

            if (photoPathWithSameIdAsUser != null)
            {
                File.Delete(photoPathWithSameIdAsUser);
            }

            return;
        }

        public static string UpdatePhotoPath(string folderName, string photoName, int userId)
        {
            photoName = photoName.Insert(0, userId.ToString() + "_");
            var absolutePath = Path.Combine(folderName, photoName);
            var relativePath = absolutePath.Substring(absolutePath.IndexOf("wwwroot") + 7).Replace('\\', '/');

            return relativePath;
        }

        public static string UpdateIngridientPhotoPath(string folderName, string photoName)
        {            
            var absolutePath = Path.Combine(folderName, photoName);
            var relativePath = absolutePath.Substring(absolutePath.IndexOf("wwwroot") + 7).Replace('\\', '/');

            return relativePath;
        }
        #endregion
    }
}
