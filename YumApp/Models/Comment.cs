﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YumApp.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }        
        [Required]
        [MinLength(1, ErrorMessage = "Minimum lenght is 1 character.")]
        [MaxLength(500, ErrorMessage = "Maximum lenght is 500 characters.")]
        public string Content { get; set; }
        public AppUser Commentator { get; set; }

        //Navigation properties
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
