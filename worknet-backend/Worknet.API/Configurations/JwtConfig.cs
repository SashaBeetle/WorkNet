namespace Worknet.Shared.Models.Auth;
public class JwtConfig
{
    public const string ConfigName = "Jwt";
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}