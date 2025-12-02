namespace API.Shared.Events;

public record JobResultEvent(
    Guid JobId,
    string Name,
    JobStatus Status,
    DateTime CreatedAtUTC,
    DateTime FinishedAtUTC,
    string? Reason = null
) : IGoEvent
{
    public static string EventName => "job-result";
}