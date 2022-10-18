using StackExchange.Redis;

namespace DemoApp.Publisher;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private const string StreamKey = "my-stream";

    public Worker(
        ILogger<Worker> logger,
        IServiceScopeFactory scopeFactory
    )
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<IDatabase>();
            var entry = await database.StreamAddAsync(
                StreamKey,
                "timestamp",
                DateTimeOffset.UtcNow.ToString("F"));

            _logger.LogInformation("Added entry {entryId} to {streamKey}", entry.ToString(), StreamKey);

            var delay = Random.Shared.Next(500, 10000);
            _logger.LogInformation("Now waiting for {delayTime}", delay);

            await Task.Delay(delay, stoppingToken);
        }
    }
}