using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models
{
    public class SignInInput
    {
        [Display(Name = "Email adresiniz"), Required, EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Şifre"), Required, PasswordPropertyText]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool IsRemember { get; set; }
    }
}
