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
    /// Executes the initialize operation.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask Initialize(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the observe element position operation.
    /// </summary>
    /// <param name="reference">The reference.</param>
    /// <param name="elementId">The element id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask ObserveElementPosition(ElementReference reference, string elementId, CancellationToken cancellationToken = default);
}