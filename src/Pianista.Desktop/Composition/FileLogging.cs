using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Pianista.Desktop.Composition;

public static class FileLogging
{
    public static IServiceCollection AddFileLogging(this IServiceCollection services)
    {
        services.AddSerilog((provider, configuration) => configuration
            .ReadFrom.Configuration(provider.GetRequiredService<IConfiguration>())
            .WriteTo.File(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Pianista",
                    "logs",
                    "log-.txt"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"));
        return services;
    }
}
