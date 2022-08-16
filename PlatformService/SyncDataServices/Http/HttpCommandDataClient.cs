using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlatformService.DTOs;
using PlatformService.Options;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly ServicesUrls _servicesUrls;

    public HttpCommandDataClient(HttpClient httpClient, IOptions<ServicesUrls> options)
    {
        _httpClient = httpClient;
        _servicesUrls = options.Value;
    }

    public async Task SendPlatformToCommand(PlatformReadDto readDto)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(readDto),
            Encoding.UTF8,
            "application/json"
            );

        try
        {
            await _httpClient.PostAsync(_servicesUrls.CommandService, httpContent);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}