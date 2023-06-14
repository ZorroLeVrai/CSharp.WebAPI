using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MonitoringApi.HealthChecks;

public class RandomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        //You can do any health check here
        int responseTimeInMs = Random.Shared.Next(300);

        if (responseTimeInMs < 100)
            return Task.FromResult(HealthCheckResult.Healthy($"The response time is excellent ({responseTimeInMs} ms)"));

        if (responseTimeInMs < 200)
            return Task.FromResult(HealthCheckResult.Degraded($"The response time is geater than expected ({responseTimeInMs} ms)"));

        return Task.FromResult(HealthCheckResult.Unhealthy($"The response time is unacceptable ({responseTimeInMs} ms)"));
    }
}
