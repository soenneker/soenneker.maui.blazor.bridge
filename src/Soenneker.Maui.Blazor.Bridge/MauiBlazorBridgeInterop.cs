using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Extensions.ValueTask;
using Soenneker.Maui.Blazor.Bridge.Abstract;
using Soenneker.Utils.CancellationScopes;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Maui.Blazor.Bridge;

///<inheritdoc cref="IMauiBlazorBridgeInterop"/>
public sealed class MauiBlazorBridgeInterop : IMauiBlazorBridgeInterop
{
    private readonly AsyncInitializer _moduleInitializer;
    private readonly CancellationScope _cancellationScope = new();

    private const string _module = "Soenneker.Maui.Blazor.Bridge/js/mauiblazorbridgeinterop.js";
    private const string _moduleNamespace = "MauiBlazorBridgeInterop";

    private readonly IResourceLoader _resourceLoader;
    private readonly IJSRuntime _jSRuntime;

    public MauiBlazorBridgeInterop(IResourceLoader resourceLoader, IJSRuntime jSRuntime)
    {
        _resourceLoader = resourceLoader;
        _jSRuntime = jSRuntime;
        _moduleInitializer = new AsyncInitializer(InitializeModule);
    }

    private async ValueTask InitializeModule(CancellationToken token)
    {
        _ = await _resourceLoader.ImportModule(_module, token);
    }

    public async ValueTask Initialize(CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
            await _moduleInitializer.Init(linked);
    }

    public async ValueTask ObserveElementPosition(ElementReference reference, string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
            await _jSRuntime.InvokeVoidAsync("MauiBlazorBridgeInterop.observeElementPosition", linked, reference, elementId);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_module).NoSync();

        await _moduleInitializer.DisposeAsync().NoSync();
        await _cancellationScope.DisposeAsync().NoSync();
    }
}