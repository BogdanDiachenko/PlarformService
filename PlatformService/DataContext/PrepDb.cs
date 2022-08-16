using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.DataContext;

public class PrepDb
{
    public static  async Task PrepPopulation(WebApplication app)
    {
        using (var servicesScope = app.Services.CreateScope())
        {
            var dbContext = servicesScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            if (dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await dbContext.Database.MigrateAsync();
            }
            
            await SeedData(dbContext);
        }
    }

    private static async Task SeedData(ApplicationDbContext dbContext)
    {
        if (!dbContext.Platforms.Any())
        {
            var platforms = new List<Platform>
            {
                new()
                {
                    Name = "Dot Net",
                    Publisher = "Microsoft",
                    Cost = "Free",
                },
                new()
                {
                    Name = "SQL Server Express",
                    Publisher = "Microsoft",
                    Cost = "Free",
                },
                new()
                {
                    Name = "Kubernetes",
                    Publisher = "Cloud Native Computing Foundation",
                    Cost = "Free",
                },
            };

            await dbContext.Platforms.AddRangeAsync(platforms);
            await dbContext.SaveChangesAsync();
        }
    }
}