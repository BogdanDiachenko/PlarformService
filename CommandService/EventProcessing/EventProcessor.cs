using System.Text.Json;
using AutoMapper;
using CommandService.Constants;
using CommandService.DTOs;
using CommandService.Models;
using CommandService.Repositories;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;
    

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public async Task ProcessEvent(string message)
    {
        var platformPublishDto = JsonSerializer.Deserialize<PlatformPublishDto>(message);

        if(platformPublishDto?.Event == EventType.Published)
        {
            Console.WriteLine("Received published event");
            await AddPlatform(platformPublishDto);
        }
        else
        {
            Console.WriteLine("Received unknown message");
        }

    }

    private async Task AddPlatform(PlatformPublishDto platformPublishDto)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
            var platform = _mapper.Map<Platform>(platformPublishDto);

            if (await repo.ExternalPlatformExists(platform.ExternalId) == false)
            {
                await repo.CreatePlatform(platform);
                await repo.SaveChanges();
            }
            else
            {
                Console.WriteLine("Platform already exists");
            }
        }
    }
}
