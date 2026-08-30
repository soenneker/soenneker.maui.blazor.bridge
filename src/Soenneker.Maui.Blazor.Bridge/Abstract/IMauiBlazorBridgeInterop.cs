using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Maui.Blazor.Bridge.Abstract;

/// <summary>
/// Defines the maui blazor bridge interop contract.
/// </summary>
public interface IMauiBlazorBridgeInterop : IAsyncDisposable
{
    /// <summary>
    /// Initializes the maui blazor bridge so it is ready for use.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the maui blazor bridge is ready for use.</returns>
    ValueTask Initialize(CancellationToken cancellationToken = default);

    /// <summary>
    /// Observes element Position.
    /// </summary>
    /// <param name="reference">Reference for the observe element position operation.</param>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the observe element position operation is complete.</returns>
    ValueTask ObserveElementPosition(ElementReference reference, string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops observing the DOM element associated with the supplied identifier.
    /// </summary>
    /// <param name="elementId">Identifier passed to <see cref="ObserveElementPosition"/>.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when browser observers and event handlers have been removed.</returns>
    ValueTask UnobserveElementPosition(string elementId, CancellationToken cancellationToken = default);
}
