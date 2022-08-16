using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;
using PlatformService;

namespace CommandService.MapperProfiles;

public class CommandProfiles : Profile
{
    public CommandProfiles()
    {
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformPublishDto, Platform>()
            .ForMember(x => x.ExternalId,opt => opt.MapFrom(dto => dto.Id));
        CreateMap<GrpcPlatformModel, Platform>();
    }  
}