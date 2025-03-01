using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Maui.Blazor.Bridge.Abstract;
using Soenneker.Utils.AsyncSingleton;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Maui.Blazor.Bridge;

public class MauiBridgeInterop : IMauiBridgeInterop
{
    private readonly AsyncSingleton _moduleInitializer;

    private const string _module = "Soenneker.Maui.Blazor.Bridge/js/mauibridgeinterop.js";

    private readonly IResourceLoader _resourceLoader;
    private readonly IJSRuntime _jSRuntime;

    public MauiBridgeInterop(IResourceLoader resourceLoader, IJSRuntime jSRuntime)
    {
        _resourceLoader = resourceLoader;
        _jSRuntime = jSRuntime;

        _moduleInitializer = new AsyncSingleton(async (token, _) =>
        {
            await resourceLoader.ImportModuleAndWaitUntilAvailable(_module, "MauiBridgeInterop", 100, token).NoSync();

            return new object();
        });
    }

    public ValueTask Init(CancellationToken cancellationToken = default)
    {
        return _moduleInitializer.Init(cancellationToken);
    }

    public async ValueTask ObserveElementPosition(ElementReference reference, string elementId, CancellationToken cancellationToken = default)
    {
        await _jSRuntime.InvokeVoidAsync("MauiBridgeInterop.observeElementPosition", cancellationToken, reference, elementId).NoSync();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _resourceLoader.DisposeModule(_module).NoSync();

        await _moduleInitializer.DisposeAsync().NoSync();
    }
}