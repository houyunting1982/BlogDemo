using System.ComponentModel.DataAnnotations;

namespace Blog.Api.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(60, MinimumLength = 4)]
        public string Password { get; set; }
    }
}
