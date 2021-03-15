using EntityLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [DisplayName("First Name")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters.")]
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [DisplayName("Last Name")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters.")]
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]        
        public string Email { get; set; }

        [Required]
        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Minimum lenght is 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Minimum lenght is 8 characters")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
