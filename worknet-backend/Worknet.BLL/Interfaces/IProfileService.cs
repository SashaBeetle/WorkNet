using Worknet.Shared.Models.DTOs;

namespace Worknet.BLL.Interfaces;
public interface IProfileService
{
    public Task<ProfileDto> CreateProfileAsync(ProfileDto profileDto);
    public Task<ProfileDto> UpdateProfileAsync(ProfileDto profileDto);
    public Task<ProfileDto?> GetProfileByUserId(string userId);
    public Task<bool> IsProfileExistForUserByUserId(string userId);
}