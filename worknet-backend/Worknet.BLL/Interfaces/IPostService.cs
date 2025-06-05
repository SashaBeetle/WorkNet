using Worknet.Shared.Models.DTOs;
using Worknet.Shared.Models.Requests;

namespace Worknet.BLL.Interfaces;
public interface IPostService
{
    public Task<PostDto> CreatePostAsync(AddPostRequest postDto);
    public Task<PostDto> DeletePostByIdAsync(string id);
    public Task<ICollection<PostDto>> GetAllPostsAsync();
    public Task<ICollection<PostDto>> GetAllPostsByDateAsync();
}