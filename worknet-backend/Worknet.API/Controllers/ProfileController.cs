using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Worknet.BLL.Interfaces;
using Worknet.Shared.Models.DTOs;

namespace Worknet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController(IProfileService profileService) : ControllerBase
{
    [HttpPost("upload-file")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromServices] IFileService fileService)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        await using var stream = file.OpenReadStream();
        var result = await fileService.UploadFileAsync(stream, file.FileName);

        return Ok(new { result });
    }

    [HttpPut]
    public async Task<IActionResult> HandleProfile([FromBody] ProfileDto profile)
    {
        if (profile is null) return BadRequest();

        bool isProfileExist = false;

        if(!string.IsNullOrEmpty(profile.Id))
            isProfileExist = await profileService.IsProfileExistForUserByUserId(profile.UserId);

        if (isProfileExist)
            profile = await profileService.UpdateProfileAsync(profile);
        else
            profile = await profileService.CreateProfileAsync(profile);


        return Ok(new { profile });
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var profile = await profileService.GetProfileWithIncludesByUserId(userId);

            if (profile is null)
                return NotFound();

            return Ok(profile);
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }

    [HttpGet("info")]
    public async Task<IActionResult> GetProfileInfo()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var profile = await profileService.GetProfileWithIncludesByUserId(userId);

            if (profile is null)
                return NotFound();

            return Ok(profile);
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }

}