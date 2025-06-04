using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Worknet.BLL.Exceptions;
using Worknet.BLL.Interfaces;
using Worknet.Core.Entities;
using Worknet.DAL;
using Worknet.Shared.Models.DTOs;

namespace Worknet.BLL.Services;
public class PostService(IMapper mapper, WorknetDbContext dbContext) : IPostService
{
    public async Task<PostDto> CreatePostAsync(PostDto postDto)
    {
        var post = mapper.Map<Post>(postDto);

        post.Id = Guid.NewGuid().ToString();

        await dbContext.Posts.AddAsync(post);
        await dbContext.SaveChangesAsync();

        return mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> DeletePostByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new WorknetException(nameof(id), "Post ID cannot be null or empty.");
        }

        var post = await dbContext.Posts.FindAsync(id);

        if (post is null)
            throw new WorknetException("Post not found.", $"Post was expected but found null for postId: {post.Id}.");

        dbContext.Posts.Remove(post);
        await dbContext.SaveChangesAsync();

        return mapper.Map<PostDto>(post);
    }

    public async Task<ICollection<PostDto>> GetAllPostsAsync()
    {
        var posts = await dbContext.Posts
                   .AsNoTracking()
                   .ToListAsync();

        return mapper.Map<ICollection<PostDto>>(posts);
    }

    public async Task<ICollection<PostDto>> GetAllPostsByDateAsync()
    {
        IQueryable<Post> query = dbContext.Posts
            .Include(x => x.User)
            .AsNoTracking();

        query = query.OrderBy(p => p.CreatedAt);
       
        var posts = await query.ToListAsync();
        return mapper.Map<ICollection<PostDto>>(posts);
    }
}