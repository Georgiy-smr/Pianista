using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(desktop.Args ?? []);
            builder.Services.AddDesktop();
            IHost host = builder.Build();
            host.Start();
            desktop.Exit += (_, _) =>
            {
                host.StopAsync().GetAwaiter().GetResult();
                host.Dispose();
            };
            desktop.MainWindow = host.Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
