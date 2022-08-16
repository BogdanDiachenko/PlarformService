using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.Repository;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]

public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;
    
    public PlatformsController(IPlatformRepository repository, IMapper mapper, ICommandDataClient commandDataClient, IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlatforms(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(5000, cancellationToken);
            var platforms = await _repository.GetAllPlatforms(cancellationToken);

            return platforms.Any() 
                ? Ok(_mapper.Map<List<PlatformReadDto>>(platforms))
                : NotFound();
        }
        catch(TaskCanceledException)
        {
            Console.WriteLine("====== Task was Cancelled =======");    
        }

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlatformById(CancellationToken cancellationToken, int id)
    {
        var platform = await _repository.GetById(cancellationToken, id);

        return platform == null
            ? NotFound()
            : Ok(_mapper.Map<PlatformReadDto>(platform));
    }   

    [HttpPost]
    public async Task<IActionResult> CreatePlatform(CancellationToken cancellationToken, PlatformCreateDto platformCreateDto)
    {
        var platform = _mapper.Map<Platform>(platformCreateDto);

        await _repository.CreatePlatform(cancellationToken, platform);
        await _repository.SaveChangesAsync(cancellationToken);

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
        
        await _commandDataClient.SendPlatformToCommand(platformReadDto);

        var platformPublishDto = _mapper.Map<PlatformPublishDto>(platformReadDto);
        platformPublishDto.Event = "Platform_Published";
        _messageBusClient.PublishNewPlatform(platformPublishDto);

        return CreatedAtRoute(new { platformReadDto.Id }, platformReadDto);
    }
}