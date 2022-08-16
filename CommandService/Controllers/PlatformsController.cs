using AutoMapper;
using CommandService.DTOs;
using CommandService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/c/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> TestInboundConnection()
    {
        Console.WriteLine("Test");

        return Ok("Test");
    }

    [HttpGet]
    public async Task<IActionResult> GetPlatforms()
    {
        var platforms = await _repository.GetAllPlatforms();
        
        return platforms.Count > 0
            ? Ok(_mapper.Map<List<PlatformReadDto>>(platforms))
            : NotFound();
    }
}