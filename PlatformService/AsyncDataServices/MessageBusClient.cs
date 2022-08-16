using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlatformService.DTOs;
using PlatformService.Options;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly RabbitMqOptions _options;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
        
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);

        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        Console.WriteLine("--> Connected to Message Bus");
    }
    
    public void PublishNewPlatform(PlatformPublishDto platformDto)
    {
        var message = JsonSerializer.Serialize(platformDto);

        if (_connection.IsOpen)
        {
            Console.WriteLine("--> Sending message...");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("--> Connection is closed...");
        }
        
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        
        _channel.BasicPublish("trigger", "", null, body);
    }

    public void Dispose()
    {
        Console.WriteLine("Message bus disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
    
    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> Connection ShutDown");
    }
}