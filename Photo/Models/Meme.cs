using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Photo.Models
{
    public class Meme
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Meme Text")]
        public string Text { get; set; }
    }
}