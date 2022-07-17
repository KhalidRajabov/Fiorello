using System.Collections.Generic;

namespace FrontToBack.ViewModels
{
    public class UserInfoVM
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string ImageURL { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActivated { get; set; }
        public List<string> Role { get; set; }
        public string About { get; set; }
    }
}
