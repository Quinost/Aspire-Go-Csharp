namespace API.Shared.Events;

public class JobAddedEvent(string name) : IGoEvent
{
    public static string EventName => "job-added";

    public Guid JobId { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = name;
    public JobStatus Status { get; private set; } = JobStatus.Pending;
    public DateTime CreatedAtUTC { get; private set; } = DateTime.UtcNow;
}
