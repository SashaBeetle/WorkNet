using AutoMapper;
using Worknet.Core.Entities;
using Worknet.Shared.Models;
using Worknet.Shared.Models.DTOs;

namespace Worknet.BLL.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserInfo>();
    }
}