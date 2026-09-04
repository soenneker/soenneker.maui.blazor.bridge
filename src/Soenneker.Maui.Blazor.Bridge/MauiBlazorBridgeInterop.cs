using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Extensions.ValueTask;
using Soenneker.Maui.Blazor.Bridge.Abstract;
using Soenneker.Utils.CancellationScopes;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Maui.Blazor.Bridge;

/// <inheritdoc cref="IMauiBlazorBridgeInterop" />
public sealed class MauiBlazorBridgeInterop : IMauiBlazorBridgeInterop
{
    private readonly CancellationScope _cancellationScope = new();

    private const string _module = "./_content/Soenneker.Maui.Blazor.Bridge/js/mauiblazorbridgeinterop.js";
    private const string _jsObserveElementPosition = "observeElementPosition";
    private const string _jsUnobserveElementPosition = "unobserveElementPosition";

    private readonly IModuleImportUtil _moduleImportUtil;

    public MauiBlazorBridgeInterop(IModuleImportUtil moduleImportUtil)
    {
        _moduleImportUtil = moduleImportUtil;
    }

    public async ValueTask Initialize(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
            await _moduleImportUtil.GetContentModuleReference(_module, linked);
    }

    public async ValueTask ObserveElementPosition(ElementReference reference, string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_module, linked);
            await module.InvokeVoidAsync(_jsObserveElementPosition, linked, reference, elementId);
        }
    }

    public async ValueTask UnobserveElementPosition(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_module, linked);
            await module.InvokeVoidAsync(_jsUnobserveElementPosition, linked, elementId);
        }
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        _cancellationScope.Cancel();

        try
        {
            await _moduleImportUtil.DisposeContentModule(_module).NoSync();
        }
        finally
        {
            await _cancellationScope.DisposeAsync().NoSync();
        }
    }
}
