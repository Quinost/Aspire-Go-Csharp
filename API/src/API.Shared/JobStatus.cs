using System.Text.Json.Serialization;

namespace API.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum JobStatus
{
    Pending,
    InProgress,
    Completed,
    Failed
}
