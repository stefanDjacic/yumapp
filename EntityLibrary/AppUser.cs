using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser()
        {
            Followers = new HashSet<User_Follows>();
            Follow = new HashSet<User_Follows>();
            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
            User_Feeds = new HashSet<User_Feed>();
        }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [DisplayName("Last Name")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters.")]
        [MaxLength(100, ErrorMessage = "Maximum lenght is 100 characters")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Date of birth")]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [Required]
        public GenderEnum Gender { get; set; }
        [MaxLength(200, ErrorMessage = "Maximum length is 200 characters.")]
        public string About { get; set; }
        [Required]
        public string PhotoPath { get; set; } = @"C:\Users\Korisnik\Desktop\YumApp Photos\DefaultUserPhoto";

        //Navigation properties
        [InverseProperty("Follower")]
        public ICollection<User_Follows> Followers { get; set; }
        [InverseProperty("Follows")]
        public ICollection<User_Follows> Follow { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<User_Feed> User_Feeds { get; set; }

        //Other
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
