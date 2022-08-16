using AutoMapper;
using Grpc.Core;
using PlatformService.Repository;

namespace PlatformService.SyncDataServices.Grpc;

public class GrpcPlatformService : PlatformServiceGrpc.PlatformServiceGrpcBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    
    public GrpcPlatformService(IPlatformRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
    {
        var platformResponse = new PlatformResponse();
        var platforms = await _repository.GetAllPlatforms(CancellationToken.None);
        
        platforms.ForEach(x => platformResponse.Platforms.Add(_mapper.Map<GrpcPlatformModel>(x)));

        return await Task.FromResult(platformResponse);
    }
}