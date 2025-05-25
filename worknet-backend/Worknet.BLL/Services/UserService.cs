using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Worknet.BLL.Exceptions;
using Worknet.BLL.Interfaces;
using Worknet.Core.Entities;
using Worknet.DAL;
using Worknet.Shared.Models;
using Worknet.Shared.Models.Auth;
using Worknet.Shared.Models.DTOs;

namespace Worknet.BLL.Services;
public class UserService(
    WorknetDbContext dbContext, 
    IMapper mapper, 
    UserManager<User> userManager,
    SignInManager<User> signInManager
    ) : IUserService
{
    public async Task<UserInfo> CreateUserAsync(RegisterUser authUser)
    {
        var user = new User
        {
            Email = authUser.UserEmail,
            UserName = authUser.UserName,
        };

        var result = await userManager.CreateAsync(user, authUser.Password);

        if (!result.Succeeded)
            throw new WorknetException("User not created.", result.Errors.ToString());

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

    public async Task<UserDto> IsUserLoggedIn(AuthUser authUser)
    {
        var user = await userManager.FindByEmailAsync(authUser.UserEmail);
        if (user is null || !await userManager.CheckPasswordAsync(user, authUser.Password))
            throw new WorknetException("User not logged In", $"User not exist or Password is incorrect {authUser.UserEmail}.");

        return mapper.Map<UserDto>(user);
    }

    public async Task UserLogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}