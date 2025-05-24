using Worknet.Shared.Models;
using Worknet.Shared.Models.Auth;
using Worknet.Shared.Models.DTOs;

namespace Worknet.BLL.Interfaces;
public interface IUserService
{
    public Task<UserDto> FindUserByNameAsync(string userName);
    public Task<UserDto?> GetUserByEmailAsync(string email);
    public Task<UserInfo> CreateUserAsync(RegisterUser authUser);
}