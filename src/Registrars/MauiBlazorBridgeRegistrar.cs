using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.CallbackRegistry.Registrars;
using Soenneker.Maui.Blazor.Bridge.Abstract;

namespace Soenneker.Maui.Blazor.Bridge.Registrars;

public static class MauiBlazorBridgeRegistrar
{
    public static IServiceCollection AddMauiBlazorBridgeAsScoped(this IServiceCollection services)
    {
        services.AddBlazorCallbackRegistryAsScoped();
        services.TryAddSingleton<IMauiBridgeInterop, MauiBridgeInterop>();

        return services;
    }
}