global using CommunityToolkit.Maui;
global using CommunityToolkit.Maui.Core.Primitives;
global using CommunityToolkit.Maui.Views;
global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Input;

global using Months18.Models;
global using Months18.Pages;
global using Months18.Services;
global using Months18.ViewModels;

global using System.Collections.ObjectModel;
global using System.ComponentModel;

using Microsoft.Extensions.Logging;

namespace Months18;

public static class MauiProgram
{
    static IServiceProvider? serviceProvider;
    public static TService? GetService<TService>()
    {
        if (serviceProvider != null)
            return serviceProvider.GetService<TService>();

        return default;
    }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        var services = builder.Services;

        services.AddSingleton<IMusicPlayerService, MusicPlayerService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<MusicPageViewModel>();
        services.AddTransient<MusicPage>();

        var app = builder.Build();
        serviceProvider = app.Services; // store service provider reference

        return app;
    }
}
