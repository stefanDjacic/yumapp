﻿using EntityLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public static class AppUserExtensionMethods
    {
        public static AppUser ToAppUserEntity(this RegisterModel model)
        {
            var output = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                PasswordHash = model.Password,
                UserName = model.UserName,
                PhotoPath = model.PhotoPath
            };

            return output;
        }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [DisplayName("First Name")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters.")]
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [DisplayName("Last Name")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters.")]
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Minimum lenght is 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Password confirmation is required.")]
        [MinLength(8, ErrorMessage = "Minimum lenght is 8 characters.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password confirmation must match with password.")]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }

        public string UserName { get; set; }

        public string PhotoPath { get; set; }
    }
}
