using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Worknet.BLL.Exceptions;
using Worknet.BLL.Interfaces;
using Worknet.Core.Entities;
using Worknet.DAL;
using Worknet.Shared.Helpers;
using Worknet.Shared.Models.DTOs;
using Worknet.Shared.Models.Requests;

namespace Worknet.BLL.Services;
public class PostService(IMapper mapper, WorknetDbContext dbContext) : IPostService
{
    public async Task<PostDto> CreatePostAsync(AddPostRequest postRequest)
    {
        var post = new Post
        {
            Data = postRequest.Data,
            UserId = UserHelper.GetCurrentUserId()
        };


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

        // 2. Отримати всі унікальні UserId з отриманих постів
        var userIds = posts
            .Where(p => p.UserId != null) // Переконатися, що UserId не null
            .Select(p => p.UserId)
            .Distinct()
            .ToList();

        // 3. Отримати всі профілі для цих UserId
        // Зберегти їх у словник для ефективного пошуку
        var userProfilesMap = new Dictionary<string, string?>();
        if (userIds.Any())
        {
            userProfilesMap = await dbContext.Profiles
                .Where(profile => userIds.Contains(profile.UserId)) // Profile має властивість UserId
                .AsNoTracking()
                .ToDictionaryAsync(profile => profile.UserId, profile => profile.ProfilePhotoId);
        }

        // 4. Змапити сутності Post на об'єкти PostDto та заповнити ProfilePhotoId
        var postDtos = new List<PostDto>();
        foreach (var post in posts)
        {
            // AutoMapper обробляє базове мапування Post на PostDto, включаючи вкладений об'єкт User
            var postDto = mapper.Map<PostDto>(post);

            // Якщо пост має UserId і для цього користувача знайдено профіль, встановити ProfilePhotoId
            if (post.UserId != null && userProfilesMap.TryGetValue(post.UserId, out var profilePhotoId))
            {
                postDto.ProfilePhotoId = profilePhotoId;
            }
            // Властивість User в PostDto (типу Worknet.Core.Entities.User) вже повинна бути змаплена
            // завдяки Include(p => p.User) та відповідній конфігурації AutoMapper.

            postDtos.Add(postDto);
        }

        return postDtos;
    }
}