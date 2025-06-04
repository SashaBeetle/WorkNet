using Worknet.Core.Entities;
using Worknet.Shared.Models;
using Worknet.Shared.Models.DTOs;
using Profile = AutoMapper.Profile;
using ProfileEntity = Worknet.Core.Entities.Profile;

namespace Worknet.BLL.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserInfo>();
        CreateMap<GoogleDriveFile, GoogleDriveFileDto>().ReverseMap();
        CreateMap<ProfileEntity, ProfileDto>().ReverseMap();
        CreateMap<Post, PostDto>().ReverseMap();

        CreateMap<EducationDto, Education>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Id) ? Guid.NewGuid().ToString() : src.Id
            ))
            .ReverseMap();

        CreateMap<ExperienceDto, Experience>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Id) ? Guid.NewGuid().ToString() : src.Id
            ))
            .ReverseMap();

        CreateMap<SkillDto, Skill>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Id) ? Guid.NewGuid().ToString() : src.Id
            ))
            .ReverseMap();
    }
}