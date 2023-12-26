using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Months18.Pages;
using Months18.Services;
using Months18.ViewModels;

namespace Months18
{
    public static class MauiProgram
    {
        static IServiceProvider? serviceProvider;
        public static TService? GetService<TService>()
        {
            if ( serviceProvider !=null)
            return serviceProvider.GetService<TService>();
            else 
                return default;
        }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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

            services.AddSingleton<MusicPlayerService>();
            services.AddSingleton<MusicPage>();

            var app= builder.Build();
            serviceProvider = app.Services; // store service provider reference
            
            return app;
        }
    }
}
