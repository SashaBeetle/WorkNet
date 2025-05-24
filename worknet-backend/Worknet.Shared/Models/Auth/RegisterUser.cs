using System.ComponentModel.DataAnnotations;

namespace Worknet.Shared.Models.Auth;
public class RegisterUser : AuthUser
{
    [Required(ErrorMessage = "User Name is required.")]
    public string? UserName { get; set; }
}