using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Worknet.Shared.Models.Auth;
public class AuthUser
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [JsonPropertyName("userEmail")]
    public string? UserEmail { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    [JsonPropertyName("password")]
    public string? Password { get; set; }
}