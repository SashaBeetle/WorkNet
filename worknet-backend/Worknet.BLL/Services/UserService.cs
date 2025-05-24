using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Worknet.BLL.Exceptions;
using Worknet.BLL.Interfaces;
using Worknet.Core.Entities;
using Worknet.DAL;
using Worknet.Shared.Helpers;
using Worknet.Shared.Models;
using Worknet.Shared.Models.Auth;
using Worknet.Shared.Models.DTOs;

namespace Worknet.BLL.Services;
public class UserService(WorknetDbContext dbContext, IMapper mapper) : IUserService
{
    public async Task<UserInfo> CreateUserAsync(RegisterUser authUser)
    {
        var user = new User
        {
            Email = authUser.UserEmail,
            UserName = authUser.UserName,
            PasswordHash = PasswordHelper.HashPassword(authUser.Password),

            NormalizedEmail = authUser.UserEmail.ToLower(),
            NormalizedUserName = authUser.UserName.ToLower(),
        };

        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return mapper.Map<UserInfo>(user);
    }

    public async Task<UserDto> FindUserByNameAsync(string userName)
    {
       var user = await dbContext.Users.Where(u => u.NormalizedUserName.Equals(userName.ToLower())).FirstOrDefaultAsync();

        if (user is null)
            throw new WorknetException("User not found.", $"User was expected but found null for UserName: {userName}.");

        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await dbContext.Users.Where(u => u.NormalizedEmail.Equals(email.ToLower())).FirstOrDefaultAsync();

        return mapper.Map<UserDto>(user);
    }
}