using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontToBack.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActivated { get; set; }
        public string About { get; set; }
        public string ImageURL { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }


        public DateTime UserCreatedTime { get; set; }
        public DateTime ConfirmedTime { get; set; }

        public List<Sale> Sales { get; set; }

    }
}
