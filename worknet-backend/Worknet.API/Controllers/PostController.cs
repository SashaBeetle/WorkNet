using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Worknet.BLL.Interfaces;
using Worknet.Shared.Models.DTOs;

namespace Worknet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostService postService) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostsForFeed()
        {
            try
            {
                var posts = await postService.GetAllPostsByDateAsync();

                if (posts is null)
                    return NotFound();

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
        {
            try
            {
                var post = await postService.CreatePostAsync(postDto);

                if (post is null)
                    return NotFound();

                return Ok(post);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }

}
