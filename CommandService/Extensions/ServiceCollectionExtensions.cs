using AutoMapper;
using CommandService.AsyncDataServices;
using CommandService.DataContext;
using CommandService.EventProcessing;
using CommandService.Options;
using CommandService.Repositories;
using CommandService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
        services.AddScoped<ICommandRepository, CommandRepository>();
        services.AddScoped<IPlatformDataClient, PlatformDataClient>(x =>
            new PlatformDataClient(
                config.GetSection("GrpcPlatform").Value,
                x.GetRequiredService<IMapper>()
            ));
        services.AddSingleton<IEventProcessor, EventProcessor>();
        
        services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("CommandsDb"));

        services.AddHostedService<MessageBusSubscriber>();

        services.Configure<RabbitMqOptions>(config.GetSection("RabbitMQ"));
        return services;
    }
}