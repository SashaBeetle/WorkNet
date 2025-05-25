using AutoMapper;
using Worknet.Core.Entities;
using Worknet.Shared.Models;
using Worknet.Shared.Models.DTOs;
using Profile = AutoMapper.Profile;

namespace Worknet.BLL.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserInfo>();
        CreateMap<GoogleDriveFile, GoogleDriveFileDto>().ReverseMap();
    }
}