global using CommunityToolkit.Maui;
global using CommunityToolkit.Maui.Core.Primitives;
global using CommunityToolkit.Maui.Views;
global using CommunityToolkit.Mvvm.ComponentModel;
global using CommunityToolkit.Mvvm.Input;

global using Months18.Helpers;
global using Months18.Models;
global using Months18.Pages;
global using Months18.Services;
global using Months18.ViewModels;

global using System.Collections.ObjectModel;
global using System.ComponentModel;
global using System.Globalization;

using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;

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
                //fonts.AddFont("Lobster-Regular.ttf", "Lobster");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if WINDOWS
                SwitchHandler.Mapper.AppendToMapping("Custom", (h, _) =>
                {
                    h.PlatformView.OffContent = string.Empty;
                    h.PlatformView.OnContent = string.Empty;

                    h.PlatformView.MinWidth = 0;
                });
#endif
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        var services = builder.Services;

        services.AddSingleton<IAudioPlayerService, AudioPlayerService>();
        services.AddSingleton<IVideoPlayerService, VideoPlayerService>();
        services.AddSingleton<ScaleAnimation>();
        services.AddTransient<SettingsPageViewModel>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<MusicPageViewModel>();
        services.AddTransient<MusicPage>();
        services.AddTransient<VideoPageViewModel>();
        services.AddTransient<VideoPage>();

        var app = builder.Build();
        serviceProvider = app.Services; // store service provider reference

        return app;
    }
}
