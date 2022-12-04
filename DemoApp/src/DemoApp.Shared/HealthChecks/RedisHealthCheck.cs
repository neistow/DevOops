using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace DemoApp.Shared.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly IDatabase _database;

    public RedisHealthCheck(IDatabase database)
    {
        _database = database;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var latency = await _database.PingAsync();
            return latency > TimeSpan.FromSeconds(2)
                ? HealthCheckResult.Degraded()
                : HealthCheckResult.Healthy();
        }
        catch
        {
            return HealthCheckResult.Unhealthy("Lost connection to redis");
        }
    }
}