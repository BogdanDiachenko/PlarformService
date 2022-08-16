using CommandService.Models;
using CommandService.Repositories;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.DataContext;

public static class PrepDb
{
    public static async Task PrepPopulation(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var grpcClient = scope.ServiceProvider.GetRequiredService<IPlatformDataClient>();
            var platforms = await grpcClient.ReturnAllPlatforms();
            await SeedData(scope.ServiceProvider.GetRequiredService<ICommandRepository>(), platforms);
        }
    }

    private static async Task SeedData(ICommandRepository repository, IEnumerable<Platform> platforms)
    {
        async void Action(Platform x)
        {
            if (await repository.ExternalPlatformExists(x.ExternalId) == false)
            {
                await repository.CreatePlatform(x);
            }
        }

        platforms.ToList().ForEach(Action);
        
        await repository.SaveChanges();
    }
}