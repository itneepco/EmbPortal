using EmbPortal.Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace EmbPortal.Shared.Requests;

public class ChangePasswordRequest
{
    [Required]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; }

    [Required]
    [ValidPassword]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}
