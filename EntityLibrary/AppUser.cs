﻿using Microsoft.AspNetCore.Http;
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

        [Required]
        [DisplayName]
        [MinLength(2)]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateCreated { get; set; } /*= DateTime.UtcNow;*/
        
        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [MaxLength(100)]
        public string About { get; set; }

        [Required]
        public string PhotoPath { get; set; } /*= @"C:\Users\Korisnik\Desktop\YumApp Photos\DefaultUserPhoto";*/

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
