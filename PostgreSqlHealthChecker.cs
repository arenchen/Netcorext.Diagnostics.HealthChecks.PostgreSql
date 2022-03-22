using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace Netcorext.Diagnostics.HealthChecks.PostgreSql;

public class PostgreSqlHealthChecker : IHealthCheck
{
    public const string DEFAULT_HEALTH_QUERY = "SELECT 1;";

    private readonly PostgreSqlHealthCheckOptions _options;

    public PostgreSqlHealthChecker(PostgreSqlHealthCheckOptions options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        if (string.IsNullOrWhiteSpace(options.Connection)) throw new ArgumentNullException(nameof(options.Connection));
        if (string.IsNullOrWhiteSpace(options.HealthQuery)) throw new ArgumentNullException(nameof(options.HealthQuery));

        _options = options;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            using var conn = new NpgsqlConnection(_options.Connection);

            await conn.OpenAsync(cancellationToken);

            using var cmd = conn.CreateCommand();

            cmd.CommandText = _options.HealthQuery;

            var result = await cmd.ExecuteNonQueryAsync(cancellationToken);

            if (result == 0)
                return HealthCheckResult.Unhealthy("No result.");

            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(e.Message, e);
        }
    }
}