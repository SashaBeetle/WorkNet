using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Worknet.BLL.Interfaces;
using Worknet.BLL.Services;
using Worknet.Shared.Models.DTOs;

namespace Worknet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var user = await userService.GetUserByIdAsync(userId);

                if (user is null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            { 
             return NotFound();
            }
        }
        [HttpGet("user/{id?}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserDto>> GetUserDetails(string? id = null) 
        {
            try
            {
                string userIdToFetch;

                if (!string.IsNullOrEmpty(id))
                {
                    userIdToFetch = id;

                    var currentAuthenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (userIdToFetch != currentAuthenticatedUserId && !User.IsInRole("Admin")) 
                    {
                        return Forbid("You are not authorized to view this user's details.");
                    }
                }
                else
                {
                    userIdToFetch = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdToFetch))
                        return Unauthorized("Unable to determine the current user's identity from the token.");
                    
                }

                var user = await userService.GetUserByIdAsync(userIdToFetch);

                if (user == null)
                {
                    return NotFound($"User with ID '{userIdToFetch}' not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while retrieving user details.");
            }
        }
    }
}