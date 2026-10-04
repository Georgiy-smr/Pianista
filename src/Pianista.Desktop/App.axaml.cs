using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pianista.Desktop.Composition;
using Pianista.Desktop.Views;

namespace Pianista.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
            {
                Args = desktop.Args ?? [],
                ContentRootPath = AppContext.BaseDirectory,
            });
            builder.Services.AddFileLogging();
            builder.Services.AddDesktop();
            IHost host = builder.Build();
            host.Start();
            ILogger<App> logger = host.Services.GetRequiredService<ILogger<App>>();
            logger.LogInformation("Pianista started");
            desktop.Exit += (_, _) =>
            {
                logger.LogInformation("Pianista is exiting");
                host.StopAsync().GetAwaiter().GetResult();
                host.Dispose();
            };
            desktop.MainWindow = host.Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
