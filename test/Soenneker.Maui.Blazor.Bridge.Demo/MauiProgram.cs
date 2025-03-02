using Microsoft.Extensions.Logging;
using Soenneker.Maui.Blazor.Bridge.Registrars;

namespace Soenneker.Maui.Blazor.Bridge.Demo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            }).ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler(typeof(MauiLabel), typeof(MauiLabelHandler));
#endif

#if WINDOWS
                   handlers.AddHandler(typeof(MauiLabel), typeof(MauiLabelHandler));
#endif

            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddMauiBlazorBridgeAsScoped();

        return builder.Build();
    }
}