using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.MapperProfiles;

public class PlatformsProfile : Profile
{
    public PlatformsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishDto>();
        CreateMap<Platform, GrpcPlatformModel>();
    }
}