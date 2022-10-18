namespace DemoApp.Shared;

public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}