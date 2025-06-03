using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Worknet.BLL.Interfaces;
using Worknet.Core.Configurations;
using Worknet.Shared.Constants;
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
        var user = await userService.IsUserLoggedIn(authUser);

        var token = JwtUtil.GenerateJwtToken(user.Id, user.UserName, _jwtConfig);

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
    {
        var userInfo = await userService.CreateUserAsync(registerUser);

        var token = JwtUtil.GenerateJwtToken(userInfo.Id, userInfo.UserName, _jwtConfig);

        return Ok(new { userInfo, token });
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout()
    {
        await userService.UserLogoutAsync();

        return Ok(new { message = ResponseMessages.LoggetOutSuccessfully });
    }
}