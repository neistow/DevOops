using DemoApp.Shared;
using DemoApp.Worker.Options;
using StackExchange.Redis;

namespace DemoApp.Worker.StartupTasks;

public class InitializeConsumerGroup : IStartupTask
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WorkerOptions _options;
    private readonly ILogger<InitializeConsumerGroup> _logger;

    public InitializeConsumerGroup(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<InitializeConsumerGroup> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _options = configuration.GetSection(WorkerOptions.Key).Get<WorkerOptions>() 
                   ?? throw new ArgumentNullException();
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _scopeFactory.CreateScope();

        var database = scope.ServiceProvider.GetRequiredService<IDatabase>();

        _logger.LogInformation("Creating consumer group {consumerGroup} for stream {streamKey}",
            _options.ConsumerName,
            _options.StreamKey);

        try
        {
            var result = await database.StreamCreateConsumerGroupAsync(
                _options.StreamKey,
                _options.GroupName,
                StreamPosition.Beginning);

            if (result)
            {
                _logger.LogInformation("Creating consumer group {consumerGroup} was created", _options.GroupName);
                return;
            }

            _logger.LogWarning("Consumer group {consumerGroup} wasn't created", _options.GroupName);
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
            // safely ignore exception since group already exists
        }
    }
}