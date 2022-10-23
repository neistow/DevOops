using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace DemoApp.Shared;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, string connectionString)
    {
        services.TryAddSingleton(_ => ConnectionMultiplexer.Connect(connectionString));
        services.TryAddScoped(sp =>
        {
            var mux = sp.GetRequiredService<ConnectionMultiplexer>();
            return mux.GetDatabase();
        });
        return services;
    }
    
    public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
        where T : class, IStartupTask
        => services.AddTransient<IStartupTask, T>();
}