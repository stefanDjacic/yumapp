using EntityLibrary;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public static class AppUserModelExtensionMethods
    {
        public static IQueryable<AppUserModel> ToAppUserModel(this IQueryable<AppUser> appUsers)
        {
            return appUsers.Select(au => au.ToAppUserModel());
        }

        public static AppUserModel ToAppUserModel(this AppUser appUser)
        {
            var appUserModel = appUser.ToAppUserModelBaseInfo();
            appUserModel.Notifications = appUser.NotificationsReceiver
                                                .ToNotificationModel()
                                                .ToList();

            return appUserModel;

            #region bad code
            //return new AppUserModel
            //{
            //    Id = appUser.Id,
            //    FirstName = appUser.FirstName,
            //    LastName = appUser.LastName,
            //    Email = appUser.Email,
            //    Username = appUser.UserName,
            //    DateOfBirth = appUser.DateOfBirth,                                
            //    Country = appUser.Country,
            //    Gender = appUser.Gender,
            //    About = appUser.About,
            //    PhotoPath = appUser.PhotoPath,
            //    Notifications = appUser.NotificationsReceiver
            //                            .ToNotificationModel()
            //                            .ToList()
            //};
            #endregion
        }

        public static AppUserModel ToAppUserModelBaseInfo(this AppUser appUser)
        {
            return new AppUserModel
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                Username = appUser.UserName,
                DateOfBirth = appUser.DateOfBirth,
                Country = appUser.Country,
                Gender = appUser.Gender,
                About = appUser.About,
                PhotoPath = appUser.PhotoPath
            };
        }

        public static AppUser ToAppUserEntity(this AppUserModel appUserModel)
        {
            return new AppUser
            {
                Id = appUserModel.Id,
                FirstName = appUserModel.FirstName,
                LastName = appUserModel.LastName,
                Email = appUserModel.Email,
                UserName = appUserModel.Username,
                DateOfBirth = appUserModel.DateOfBirth,
                Country = appUserModel.Country,
                Gender = appUserModel.Gender,
                About = appUserModel.About,
                PhotoPath = appUserModel.PhotoPath
            };
        }

        public static void SetDefaultPhotoPathAndUserName(this AppUser appUser)
        {
            appUser.PhotoPath = @"/Photos/DefaultUserPhoto.png";
            appUser.SetDefaultUserName();
        }

        public static void SetDefaultUserName(this AppUser appUser)
        {
            appUser.UserName = appUser.Email;
        }
    }

    public class AppUserModel
    {
        public AppUserModel()
        {
            Notifications = new List<NotificationModel>();
        }

        public int Id { get; set; }

        [Required]
        [DisplayName("First name")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters.")]
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters.")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last name")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters.")]
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters.")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [MaxLength(256, ErrorMessage = "Maximum lenght is 256 characters.")]
        public string Email { get; set; }

        [Required]
        [Compare("Email", ErrorMessage = "Username must match with email.")]
        public string Username { get; set; }        

        [Required]
        [DataType(DataType.Date)]        
        [DisplayName("Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } /*= DateTime.UtcNow;*/

        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [MinLength(1, ErrorMessage = "Minimum lenght is 1 character.")]
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters.")]
        public string About { get; set; }

        [Required]
        public string PhotoPath { get; set; } /*= @"C:\Users\Korisnik\Desktop\YumApp Photos\DefaultUserPhoto";*/

        public IEnumerable<NotificationModel> Notifications { get; set; }

        public bool IsBeingFollowed { get; set; }

        public IFormFile Photo { get; set; }
    }
}
