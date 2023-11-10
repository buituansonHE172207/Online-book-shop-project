using System.ComponentModel.DataAnnotations;

namespace ShoppingWebApp.Models
{
    public class UserEditModel
    {
        public int? UserId { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Avatar { get; set; }

        [Phone]
        [Display(Name = "Phone")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Birthday")]
        [Required]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public bool? Gender { get; set; }

        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Display(Name = "New Password")]
        [StringLength(100, MinimumLength = 8)]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirm")]
        [StringLength(100, MinimumLength = 8)]
        public string? Confirm { get; set; }

    }

}
