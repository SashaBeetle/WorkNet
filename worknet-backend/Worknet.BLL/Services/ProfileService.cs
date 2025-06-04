using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Worknet.BLL.Exceptions;
using Worknet.BLL.Interfaces;
using Worknet.Core.Entities;
using Worknet.DAL;
using Worknet.Shared.Models.DTOs;
using Profile = Worknet.Core.Entities.Profile;

namespace Worknet.BLL.Services;
public class ProfileService(IMapper mapper, WorknetDbContext dbContext) : IProfileService
{
    public async Task<ProfileDto> CreateProfileAsync(ProfileDto profileDto)
    {
        var profile = mapper.Map<Profile>(profileDto);

        profile.Id = Guid.NewGuid().ToString();
        
        await dbContext.Profiles.AddAsync(profile);
        await dbContext.SaveChangesAsync();

        return mapper.Map<ProfileDto>(profile);
    }

    public async Task<ProfileDto?> GetProfileByUserId(string userId)
    {
        if (userId is null || string.IsNullOrEmpty(userId))
            throw new WorknetException("UserId cannot be null or empty.");

        var profile = dbContext.Profiles.Where(e => e.UserId.Equals(userId))
            .FirstOrDefault();

        return mapper.Map<ProfileDto>(profile);
    }

    public async Task<ProfileDto?> GetProfileWithIncludesByUserId(string userId)
    {
        if (userId is null || string.IsNullOrEmpty(userId))
            throw new WorknetException("UserId cannot be null or empty.");

        var profile = dbContext.Profiles.Where(e => e.UserId.Equals(userId))
            .Include(x => x.Experiences)
            .Include(x => x.Educations)
            .Include(x => x.Skills)
            .Include(x => x.User)
            .FirstOrDefault();

        return mapper.Map<ProfileDto>(profile);
    }

    public async Task<ProfileDto?> GetProfileWithUserByUserId(string userId)
    {
        if (userId is null || string.IsNullOrEmpty(userId))
            throw new WorknetException("UserId cannot be null or empty.");

        var profile = dbContext.Profiles.Where(e => e.UserId.Equals(userId))
            .Include(x => x.User)
            .FirstOrDefault();

        return mapper.Map<ProfileDto>(profile);
    }

    public async Task<bool> IsProfileExistForUserByUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return false; 
        
        bool profileExists = await dbContext.Profiles.AnyAsync(p => p.UserId == userId);

        return profileExists;
    }

    public async Task<ProfileDto> UpdateProfileAsync(ProfileDto profileDto)
    {
        if (profileDto is null || string.IsNullOrEmpty(profileDto.Id))
            throw new WorknetException("Profile cannot be null.");

        var existingProfile = await dbContext.Profiles
                                        .FirstOrDefaultAsync(p => p.Id == profileDto.Id);

        if (existingProfile is null)
            throw new WorknetException("Profile not found.", $"Profile was expected but found null for profileId: {profileDto.Id}.");

        mapper.Map(profileDto, existingProfile);

        var currentSkills = await dbContext.Skills
                                    .Where(s => s.ProfileId == existingProfile.Id)
                                    .ToListAsync();
        if (currentSkills.Any())
        {
            dbContext.Skills.RemoveRange(currentSkills);
        }

        var newMappedSkills = new List<Skill>();
        if (profileDto.Skills != null && profileDto.Skills.Any())
        {
            foreach (var skillDto in profileDto.Skills)
            {
                var newSkill = mapper.Map<Skill>(skillDto);
                newSkill.Id = Guid.NewGuid().ToString(); 
                newSkill.ProfileId = existingProfile.Id;
                newMappedSkills.Add(newSkill);
            }
            await dbContext.Skills.AddRangeAsync(newMappedSkills);
        }
        existingProfile.Skills = newMappedSkills; 

        var currentExperiences = await dbContext.Experiences
                                        .Where(e => e.ProfileId == existingProfile.Id)
                                        .ToListAsync();
        if (currentExperiences.Any())
        {
            dbContext.Experiences.RemoveRange(currentExperiences);
        }

        var newMappedExperiences = new List<Experience>();
        if (profileDto.Experiences != null && profileDto.Experiences.Any())
        {
            foreach (var expDto in profileDto.Experiences)
            {
                var newExperience = mapper.Map<Experience>(expDto);
                newExperience.Id = Guid.NewGuid().ToString(); 
                newExperience.ProfileId = existingProfile.Id; 
                newMappedExperiences.Add(newExperience);
            }
            await dbContext.Experiences.AddRangeAsync(newMappedExperiences);
        }
        existingProfile.Experiences = newMappedExperiences;

        var currentEducations = await dbContext.Educations
                                        .Where(edu => edu.ProfileId == existingProfile.Id)
                                        .ToListAsync();
        if (currentEducations.Any())
        {
            dbContext.Educations.RemoveRange(currentEducations);
        }

        var newMappedEducations = new List<Education>();
        if (profileDto.Educations != null && profileDto.Educations.Any())
        {
            foreach (var eduDto in profileDto.Educations)
            {
                var newEducation = mapper.Map<Education>(eduDto);
                newEducation.Id = Guid.NewGuid().ToString();
                newEducation.ProfileId = existingProfile.Id;
                newMappedEducations.Add(newEducation);
            }
            await dbContext.Educations.AddRangeAsync(newMappedEducations);
        }
        existingProfile.Educations = newMappedEducations; 

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {

            throw new WorknetException("Can't save DataBase", ex);
        }

        return mapper.Map<ProfileDto>(existingProfile);
    }
}