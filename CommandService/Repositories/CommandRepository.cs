using CommandService.DataContext;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Repositories;

public class CommandRepository : ICommandRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CommandRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<bool> SaveChanges()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<IList<Platform>> GetAllPlatforms()
    {
        return await _dbContext.Platforms.ToListAsync();
    }

    public async Task<bool> PlatformExists(int id)
    {
        return await _dbContext.Platforms.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> ExternalPlatformExists(int externalId)
    {
        return await _dbContext.Platforms.AnyAsync(x => x.ExternalId == externalId);
    }

    public async Task CreatePlatform(Platform platform)
    {
        if (platform != null)
        {
            await _dbContext.AddAsync(platform);
        }
    }

    public async Task<IList<Command>> GetAllPlatformCommands(int platformId)
    {
        return await _dbContext.Commands
            .Where(c => c.PlatformId == platformId)
            .OrderBy(c => c.Platform.Name)
            .ToListAsync();
    }

    public async Task<Command> GetCommand(int platformId, int commandId)
    {
        return await _dbContext.Commands
            .FirstOrDefaultAsync(c => c.PlatformId == platformId && c.Id == commandId);
    }

    public async Task CreateCommand(int platformId, Command command)
    {
        if (command != null)
        {
            command.PlatformId = platformId;
            await _dbContext.Commands.AddAsync(command);
        }
    }
}