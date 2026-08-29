using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.CallbackRegistry.Registrars;
using Soenneker.Maui.Blazor.Bridge.Abstract;

namespace Soenneker.Maui.Blazor.Bridge.Registrars;

/// <summary>
/// Represents the maui blazor bridge registrar.
/// </summary>
public static class MauiBlazorBridgeRegistrar
{
    /// <summary>
    /// Registers Maui Blazor Bridge with a scoped lifetime.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddMauiBlazorBridgeAsScoped(this IServiceCollection services)
    {
        services.AddBlazorCallbackRegistryAsScoped().TryAddScoped<IMauiBlazorBridgeInterop, MauiBlazorBridgeInterop>();

        return services;
    }
}
