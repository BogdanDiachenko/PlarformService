using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.DataContext;
using PlatformService.Options;
using PlatformService.Repository;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config, bool isDevelopment)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddGrpc();

        Action<DbContextOptionsBuilder> options = isDevelopment 
            ? opt => opt.UseInMemoryDatabase("PlatformService")
            : opt => opt.UseSqlServer(config.GetConnectionString("PlatformsConnection"));
            
        services.AddDbContext<ApplicationDbContext>(options);

        services.AddScoped<IPlatformRepository, PlatformRepository>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();

        services.Configure<ServicesUrls>(config.GetSection("ServiceUrls"));
        services.Configure<RabbitMqOptions>(config.GetSection("RabbitMQ"));

        return services;
    }
}