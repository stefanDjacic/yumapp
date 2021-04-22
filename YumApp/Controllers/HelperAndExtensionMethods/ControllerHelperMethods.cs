using EntityLibrary;
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

        public static async Task SavePhoto(IFormFile photo, string folderPath, int userId)
        {
            if (photo == null)
            {
                return;
            }

            if (photo.Length == 0)
            {
                return; 
            }

            var photoName = photo.FileName.Insert(0, userId.ToString() + "_");
            var fullPath = Path.Combine(folderPath + photoName);

            //Deletes old photo
            DeletePhoto(userId, fullPath);

            //Creates updated photo
            using (var stream = File.Create(fullPath))
            {
                await photo.CopyToAsync(stream);
            }

            return;
        }

        private static void DeletePhoto(int userId, string filePath)
        {
            //var photoId = filePath.Substring(filePath.LastIndexOf('\\') + 1, userId.ToString().Length);

            //Gets user photo path with the same id value as user's who is updating profile photo if it exists
            var photoPathWithSameIdAsUser = Directory.GetFiles(filePath.Substring(0, filePath.LastIndexOf('\\')))
                                                 .SingleOrDefault(f => f.Substring(filePath.LastIndexOf('\\') + 1, userId.ToString().Length) == userId.ToString());

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
    }
}
