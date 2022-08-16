using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.SyncDataServices.Grpc;

public class PlatformDataClient : IPlatformDataClient
{
    private readonly string _grpcPlatformUrl;
    private readonly IMapper _mapper;

    public PlatformDataClient(string grpcPlatformUrl, IMapper mapper)
    {
        _grpcPlatformUrl = grpcPlatformUrl;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Platform>> ReturnAllPlatforms()
    {
        Console.WriteLine("--> Calling GRPC");

        var channel = GrpcChannel.ForAddress(_grpcPlatformUrl);
        var client = new PlatformServiceGrpc.PlatformServiceGrpcClient(channel);

        try
        {
            var resp = await client.GetAllPlatformsAsync(new GetAllRequest());
            return _mapper.Map<IEnumerable<Platform>>(resp.Platforms);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
    }
}