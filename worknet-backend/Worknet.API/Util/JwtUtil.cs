using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Worknet.Shared.Models.Auth;

namespace Worknet.Shared.Helpers;
public static class JwtUtil
{
    public static string GenerateJwtToken(string userEmail, string userName, JwtConfig jwtConfig)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, userEmail),
            new (ClaimTypes.Name, userName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}