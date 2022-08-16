using PlatformService.Models;

namespace PlatformService.Repository;

public interface IPlatformRepository
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);

    Task<List<Platform>> GetAllPlatforms(CancellationToken cancellationToken);

    Task<Platform> GetById(CancellationToken cancellationToken, int id);

    Task<bool> CreatePlatform(CancellationToken cancellationToken, Platform platform);
}