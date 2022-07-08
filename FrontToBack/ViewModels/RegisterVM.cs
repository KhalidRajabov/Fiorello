using System.ComponentModel.DataAnnotations;

namespace FrontToBack.ViewModels
{
    public class RegisterVM
    {
        [Required, StringLength(100)]
        public string Fullname { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        
        
        [Required, StringLength(100), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, StringLength(16), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, StringLength(16), DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
