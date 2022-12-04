using DemoApp.Publisher;
using DemoApp.Shared;
using DemoApp.Shared.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

services.AddHealthChecks().AddCommonHealthChecks();

var redisConnectionString = configuration.GetConnectionString("Redis")
                            ?? throw new ArgumentNullException();
services.AddRedis(redisConnectionString);

services.AddHostedService<Worker>();

var host = builder.Host;
host.UseSerilogWithElastic(typeof(Program).Assembly.GetName().Name!);

var app = builder.Build();

app.MapCommonHealthChecks();

await app.RunWithTasksAsync();

public partial class Program
{
}