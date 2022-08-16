using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;
using CommandService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/platforms/{platformId}/[controller]")]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCommandsForPlatform(int platformId)
    {
        if (!await _repository.PlatformExists(platformId)) return NotFound();
        
        var commands = await _repository.GetAllPlatformCommands(platformId);

        return commands.Count > 0
            ? Ok(_mapper.Map<List<CommandReadDto>>(commands))
            : NotFound();
    }

    [HttpGet("{commandId}")]
    public async Task<IActionResult> GetCommandForPlatform(int platformId, int commandId)
    {
        if (!await _repository.PlatformExists(platformId)) return NotFound();

        var command = await _repository.GetCommand(platformId, commandId);

        return command != null
            ? Ok(_mapper.Map<CommandReadDto>(command))
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        if (!await _repository.PlatformExists(platformId)) return NotFound();

        var command = _mapper.Map<Command>(commandDto);

        await _repository.CreateCommand(platformId, command);
        await _repository.SaveChanges();

        return CreatedAtRoute(new { platformId, command.Id }, _mapper.Map<CommandReadDto>(command));
    }
}