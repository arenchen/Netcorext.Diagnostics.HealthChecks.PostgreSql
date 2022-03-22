using Microsoft.Extensions.Diagnostics.HealthChecks;
using Netcorext.Diagnostics.HealthChecks.Postgresql;

namespace Microsoft.Extensions.DependencyInjection;

public static class HealthChecksBuilderExtensions
{
    private const string NAME = "PostgreSQL";

    public static IHealthChecksBuilder AddPostgreSql(this IHealthChecksBuilder builder, PostgreSqlHealthCheckOptions options, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default, TimeSpan? timeout = default)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

        return builder.Add(new HealthCheckRegistration(name ?? NAME,
                                                       provider => new PostgreSqlHealthChecker(options),
                                                       failureStatus,
                                                       tags,
                                                       timeout));
    }

    public static IHealthChecksBuilder AddPostgreSql(this IHealthChecksBuilder builder, Func<IServiceProvider, PostgreSqlHealthCheckOptions> factory, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default, TimeSpan? timeout = default)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        return builder.Add(new HealthCheckRegistration(name ?? NAME,
                                                       provider => new PostgreSqlHealthChecker(factory.Invoke(provider)),
                                                       failureStatus,
                                                       tags,
                                                       timeout));
    }
}