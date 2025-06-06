﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Worknet.Core.Configurations;
using Worknet.Shared.Models.Auth;

namespace Worknet.Shared.Helpers;
public static class JwtUtil
{
    public static string GenerateJwtToken(string userId, string userName, JwtConfig jwtConfig)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, userId),
            new (ClaimTypes.Name, userName.ToUpper())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(100),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}