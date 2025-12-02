using API.Infrastructure.Database;
using API.Shared.Entities;
using API.Shared.Events;

namespace API.Infrastructure.Consumers;

public class JobResultConsumer(ApiDbContext dbContext) : IConsumer<JobResultEvent>
{
    public async Task Consume(ConsumeContext<JobResultEvent> context)
    {
        var job = context.Message;
        var result = new JobResult()
        {
            JobId = job.JobId,
            Name = job.Name,
            Status = job.Status.ToString(),
            Reason = job.Reason,
            CreatedAt = job.CreatedAtUTC,
            FinishedAt = job.FinishedAtUTC,
        };

        await dbContext.JobResults.AddAsync(result);
        await dbContext.SaveChangesAsync();
    }
}
