using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Maui.Blazor.Bridge.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Asyncs.Initializers;

namespace Soenneker.Maui.Blazor.Bridge;

///<inheritdoc cref="IMauiBlazorBridgeInterop"/>
public class MauiBlazorBridgeInterop : IMauiBlazorBridgeInterop
{
    private readonly AsyncInitializer _moduleInitializer;

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

    private ValueTask InitializeModule(CancellationToken token)
    {
        return _resourceLoader.ImportModuleAndWaitUntilAvailable(_module, _moduleNamespace, 100, token);
    }

    public ValueTask Initialize(CancellationToken cancellationToken = default)
    {
        return _moduleInitializer.Init(cancellationToken);
    }

    public ValueTask ObserveElementPosition(ElementReference reference, string elementId, CancellationToken cancellationToken = default)
    {
        return _jSRuntime.InvokeVoidAsync("MauiBlazorBridgeInterop.observeElementPosition", cancellationToken, reference, elementId);
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _resourceLoader.DisposeModule(_module).NoSync();

        await _moduleInitializer.DisposeAsync().NoSync();
    }
}