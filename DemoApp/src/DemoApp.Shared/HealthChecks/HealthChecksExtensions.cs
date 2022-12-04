using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp.Shared.HealthChecks;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddCommonHealthChecks(this IHealthChecksBuilder builder)
    {
        return builder.AddCheck<RedisHealthCheck>("Redis", tags: new[] { HealthCheckTags.Liveness });
    }

    public static void MapCommonHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        const string healthRoutePrefix = "/healthz";
        endpoints.MapHealthChecks($"{healthRoutePrefix}/startup", new HealthCheckOptions
        {
            Predicate = _ => false,
        });
        endpoints.MapHealthChecks($"{healthRoutePrefix}/ready", new HealthCheckOptions
        {
            Predicate = _ => false,
        });
        endpoints.MapHealthChecks($"{healthRoutePrefix}/live", new HealthCheckOptions
        {
            Predicate = hc => hc.Tags.Contains(HealthCheckTags.Liveness)
        });
    }
}