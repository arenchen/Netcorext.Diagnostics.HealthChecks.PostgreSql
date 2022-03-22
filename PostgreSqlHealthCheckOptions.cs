namespace Netcorext.Diagnostics.HealthChecks.Postgresql;

public class PostgreSqlHealthCheckOptions
{
    public const string DEFAULT_HEALTH_QUERY = "SELECT 1;";

    public string Connection { get; set; }
    public string HealthQuery { get; set; } = DEFAULT_HEALTH_QUERY;
}