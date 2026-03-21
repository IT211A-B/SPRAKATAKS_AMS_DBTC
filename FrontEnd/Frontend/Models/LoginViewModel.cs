using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}