using DemoApp.Worker.Options;
using StackExchange.Redis;

namespace DemoApp.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WorkerOptions _options;

    public Worker(
        ILogger<Worker> logger,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _options = configuration.GetSection(WorkerOptions.Key).Get<WorkerOptions>()
                   ?? throw new ArgumentException("Worker option section is not defined");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.UtcNow);

            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<IDatabase>();
            var messages = await database.StreamReadGroupAsync(
                _options.StreamKey,
                _options.GroupName,
                _options.ConsumerName,
                StreamPosition.NewMessages,
                _options.ProcessAmount,
                true);

            if (!messages.Any())
            {
                await Task.Delay(1000, stoppingToken);
                continue;
            }

            foreach (var message in messages)
            {
                _logger.LogInformation("Processing message {messageId}", message.Id.ToString());
                await database.StreamAcknowledgeAsync(_options.StreamKey, _options.GroupName, message.Id);
                _logger.LogInformation("Processed message {messageId}", message.Id.ToString());
            }
        }
    }
}