using Microsoft.Extensions.DependencyInjection;
using Pianista.Desktop.ViewModels;
using Pianista.Desktop.Views;

namespace Pianista.Desktop.Composition;

public static class DesktopServices
{
    public static IServiceCollection AddDesktop(this IServiceCollection services)
    {
        services.AddMediator();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainViewModel>(),
        });
        return services;
    }
}
