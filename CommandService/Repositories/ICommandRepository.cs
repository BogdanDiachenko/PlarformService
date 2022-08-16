using CommandService.Models;

namespace CommandService.Repositories;

public interface ICommandRepository
{
    Task<bool> SaveChanges();
    
    Task<IList<Platform>> GetAllPlatforms();

    Task<bool> PlatformExists(int id);

    Task<bool> ExternalPlatformExists(int externalId);

    Task CreatePlatform(Platform platform);

    Task<IList<Command>> GetAllPlatformCommands(int platformId);

    Task<Command> GetCommand(int platformId, int commandId);

    Task CreateCommand(int platformId, Command command);
}