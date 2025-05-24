using System.ComponentModel.DataAnnotations;

namespace Worknet.Shared.Models.Auth;
public class AuthUser
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? UserEmail { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    public string? Password { get; set; }
}