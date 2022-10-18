using DemoApp.Publisher;
using DemoApp.Shared;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var cfg = ctx.Configuration;

        var redisConnectionString = cfg.GetConnectionString("Redis");
        services.AddRedis(redisConnectionString);

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunWithTasksAsync();

public partial class Program { }