using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FrontToBack.ViewModels
{
    public class RoleVM
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public List<IdentityRole> Roles{ get; set; }

        public IList<string> UserRoles { get; set; }
    }
}
