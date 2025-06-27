using Ambev.DeveloperEvaluation.Application.Publisher;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMQPublisher : IMessagePublisher, IAsyncDisposable, IDisposable
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly SemaphoreSlim _connectionSemaphore;
    private readonly JsonSerializerOptions _jsonOptions;

    private IConnection? _connection;
    private IChannel? _channel;
    private bool _disposed;

    public RabbitMQPublisher(IOptions<RabbitMQSettings> settings, ILogger<RabbitMQPublisher> logger)
    {
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _connectionSemaphore = new SemaphoreSlim(1, 1);
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    private async Task<IChannel> GetChannelAsync()
    {
        if (_channel != null && _channel.IsOpen)
            return _channel;

        await _connectionSemaphore.WaitAsync();
        try
        {
            if (_channel != null && _channel.IsOpen)
                return _channel;

            await InitializeConnectionAsync();
            return _channel!;
        }
        finally
        {
            _connectionSemaphore.Release();
        }
    }

    private async Task InitializeConnectionAsync()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            _logger.LogInformation("Conexão com RabbitMQ estabelecida com sucesso em {HostName}:{Port}",
                _settings.HostName, _settings.Port);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao conectar com RabbitMQ em {HostName}:{Port}",
                _settings.HostName, _settings.Port);
            throw new InvalidOperationException("Falha ao conectar com RabbitMQ", ex);
        }
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(queueName);
        ArgumentNullException.ThrowIfNull(message);

        ObjectDisposedException.ThrowIf(_disposed, this);

        try
        {
            var channel = await GetChannelAsync();

            // Declara a fila se não existir
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var messageBody = JsonSerializer.Serialize(message, _jsonOptions);
            var body = Encoding.UTF8.GetBytes(messageBody);

            var properties = new BasicProperties
            {
                Persistent = true,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json",
                ContentEncoding = "utf-8"
            };

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: queueName,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

            _logger.LogDebug("Mensagem publicada na fila {QueueName} com ID {MessageId}",
                queueName, properties.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar mensagem na fila {QueueName}", queueName);
            throw new MessagePublishException($"Falha ao publicar mensagem na fila '{queueName}'", ex);
        }
    }

    public async Task PublishEventAsync<T>(string eventName, T eventData)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventName);
        ArgumentNullException.ThrowIfNull(eventData);

        var eventMessage = new EventMessage<T>
        {
            EventName = eventName,
            EventId = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Data = eventData,
            Source = Environment.MachineName,
            Version = "1.0"
        };

        var queueName = $"events.{eventName.ToLowerInvariant()}";
        await PublishAsync(queueName, eventMessage);

        _logger.LogInformation("Evento {EventName} publicado com ID {EventId}",
            eventName, eventMessage.EventId);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        try
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }

            _connectionSemaphore?.Dispose();
            _logger.LogInformation("RabbitMQ Publisher disposed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro durante dispose do RabbitMQ Publisher");
        }
        finally
        {
            _disposed = true;
        }
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}