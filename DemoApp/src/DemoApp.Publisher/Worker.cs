using DemoApp.Publisher.Options;
using StackExchange.Redis;

namespace DemoApp.Publisher;

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
        _options = configuration.GetSection(WorkerOptions.Key).Get<WorkerOptions>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<IDatabase>();
            var entry = await database.StreamAddAsync(
                _options.StreamKey,
                "timestamp",
                DateTimeOffset.UtcNow.ToString("F"));

            _logger.LogInformation("Added entry {entryId} to {streamKey}", 
                entry.ToString(),
                _options.StreamKey);

            var delay = Random.Shared.Next(500, 10000);
            _logger.LogInformation("Now waiting for {delayTime}", delay);

            await Task.Delay(delay, stoppingToken);
        }
    }
}