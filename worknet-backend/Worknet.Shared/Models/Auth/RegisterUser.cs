using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Worknet.Shared.Models.Auth;
public class RegisterUser : AuthUser
{
    [Required(ErrorMessage = "User Name is required.")]
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}