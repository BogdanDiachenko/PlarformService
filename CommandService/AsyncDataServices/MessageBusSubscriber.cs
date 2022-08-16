using System.Text;
using CommandService.EventProcessing;
using CommandService.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly RabbitMqOptions _options;
    private readonly IEventProcessor _eventProcessor;
    private readonly IConnection _connection;
    private readonly IModel _chanel;
    private readonly string _queueName;

    public MessageBusSubscriber(IOptions<RabbitMqOptions> options, IEventProcessor eventProcessor)
    {
        _eventProcessor = eventProcessor;
        _options = options.Value;
        
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port
        };

        _connection = factory.CreateConnection();
        _chanel = _connection.CreateModel();
        _chanel.ExchangeDeclare("trigger", ExchangeType.Fanout);
        _queueName = _chanel.QueueDeclare().QueueName;
        _chanel.QueueBind(_queueName, "trigger", "");

        Console.WriteLine("--> Listening queue");

        _connection.ConnectionShutdown += HandleConnectionShutdown;
    }

    private void HandleConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        throw new NotImplementedException();
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_chanel);
        
        consumer.Received += (ModuleHandle, eventArgs) =>
        {
            Console.WriteLine("Event received");

            var body = eventArgs.Body;
            var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

            _eventProcessor.ProcessEvent(notificationMessage);
        };

        _chanel.BasicConsume(_queueName, true, consumer);
        
        return Task.CompletedTask;
    }
}