using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class SignUpModel
    {
        [StringLength(100, MinimumLength = 5)]
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
        [Display(Name = "Confirm")]
        [StringLength(100, MinimumLength = 8)]
        [Required]
        public string Confirm { get; set; }
    }
}
