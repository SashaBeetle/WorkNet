using Worknet.Shared.Models.DTOs;

namespace Worknet.BLL.Interfaces;
public interface IPostService
{
    public Task<PostDto> CreatePostAsync(PostDto postDto);
    public Task<PostDto> DeletePostByIdAsync(string id);
    public Task<ICollection<PostDto>> GetAllPostsAsync();
    public Task<ICollection<PostDto>> GetAllPostsByDateAsync();
}