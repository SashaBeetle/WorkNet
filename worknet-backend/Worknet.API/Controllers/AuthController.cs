using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Worknet.BLL.Interfaces;
using Worknet.Shared.Helpers;
using Worknet.Shared.Models.Auth;

namespace Worknet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserService userService, IOptions<JwtConfig> jwtOptions) : ControllerBase
{
    private readonly JwtConfig _jwtConfig = jwtOptions.Value;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthUser authUser)
    {
        var user = await userService.GetUserByEmailAsync(authUser.UserEmail);
        if (user is null || PasswordHelper.VerifyPassword(user.PasswordHash, authUser.Password))
            return Unauthorized();
        
        var token = JwtUtil.GenerateJwtToken(user.NormalizedEmail, user.NormalizedUserName, _jwtConfig);

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
    {
        var userInfo = await userService.CreateUserAsync(registerUser);

        var token = JwtUtil.GenerateJwtToken(userInfo.Email.ToLower(),userInfo.UserName.ToLower(), _jwtConfig);

        return Ok(new { userInfo, token });
    }
}