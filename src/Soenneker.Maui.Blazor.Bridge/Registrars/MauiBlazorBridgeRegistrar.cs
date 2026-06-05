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
    /// Adds maui blazor bridge as scoped.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The result of the operation.</returns>
    public static IServiceCollection AddMauiBlazorBridgeAsScoped(this IServiceCollection services)
    {
        services.AddBlazorCallbackRegistryAsScoped().TryAddScoped<IMauiBlazorBridgeInterop, MauiBlazorBridgeInterop>();

        return services;
    }
}