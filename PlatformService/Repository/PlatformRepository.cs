using Microsoft.EntityFrameworkCore;
using PlatformService.DataContext;
using PlatformService.Models;

namespace PlatformService.Repository;

public class PlatformRepository : IPlatformRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PlatformRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<List<Platform>> GetAllPlatforms(CancellationToken cancellationToken)
    {
        return await _dbContext.Platforms.ToListAsync(cancellationToken);
    }

    public async Task<Platform> GetById(CancellationToken cancellationToken, int id)
    {
        return await _dbContext.Platforms.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> CreatePlatform(CancellationToken cancellationToken ,Platform platform)
    {
        await _dbContext.Platforms.AddAsync(platform, cancellationToken);

        return await SaveChangesAsync(cancellationToken);
    }
}