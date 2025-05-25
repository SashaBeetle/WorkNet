using Microsoft.AspNetCore.Mvc;
using Worknet.BLL.Interfaces;
using Worknet.Shared.Interfaces;

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

        var result2 = await fileService.GetFileByIdAsync(result.Id);

        return Ok(new { result });
    }
}