using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            Followers = new List<Followers>();
            Followings = new List<Following>();
            Posts = new List<Post>();
        }

        [Required(ErrorMessage ="First name is required.")]
        [DisplayName("First Name")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [DisplayName("Last Name")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters.")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Date of birth")]        
        public DateTime DateOfBirth { get; set; }
        [Required]
        public GenderEnum Gender { get; set; }
        [MaxLength(200, ErrorMessage = "Maximum length is 200 characters.")]
        public string About { get; set; }

        //Navigation properties
        public List<Followers> Followers { get; set; }
        public List<Following> Followings { get; set; }
        public List<Post> Posts { get; set; }
    }
}
