using DemoApp.Shared;
using DemoApp.Worker;
using DemoApp.Worker.StartupTasks;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var cfg = ctx.Configuration;

        var redisConnectionString = cfg.GetConnectionString("Redis");
        services.AddRedis(redisConnectionString);

        services.AddStartupTask<InitializeConsumerGroupTask>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunWithTasksAsync();

public partial class Program { }