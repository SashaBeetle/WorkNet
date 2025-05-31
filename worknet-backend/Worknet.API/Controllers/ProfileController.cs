using Microsoft.AspNetCore.Mvc;
using Worknet.BLL.Interfaces;

namespace Worknet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
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
    //[HttpPost]
    //public IActionResult CreateProfile([FromBody] ProfileDto profile)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    // Save profile to DB or do something
    //    return Ok(new { message = "Profile created", profile });
    //}

}