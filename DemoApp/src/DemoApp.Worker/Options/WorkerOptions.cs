namespace DemoApp.Worker.Options;

public class WorkerOptions
{
    public const string Key = "WorkerOptions";

    public string StreamKey { get; set; } = null!;
    public string ConsumerName { get; set; } = null!;
    public string GroupName { get; set; } = null!;
    public int ProcessAmount { get; set; } = 1;
}