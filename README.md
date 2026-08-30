# Soenneker.Maui.Blazor.Bridge
[![](https://img.shields.io/nuget/v/soenneker.maui.blazor.bridge.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.maui.blazor.bridge/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.maui.blazor.bridge/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.maui.blazor.bridge/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.maui.blazor.bridge.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.maui.blazor.bridge/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.maui.blazor.bridge/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.maui.blazor.bridge/actions/workflows/codeql.yml)

Positions native .NET MAUI views over matching elements inside a MAUI `BlazorWebView`.


---

## Features  

- Embed MAUI components directly inside **BlazorWebView** like HTML elements.  
- Maintain a structured overlay system for native elements.  
- Provides **typed** and **generic** bridges for flexible component integration.  

---

## Installation  

Install the package via NuGet:

```sh
dotnet add package Soenneker.Maui.Blazor.Bridge
```

Register the interop in `CreateMauiApp`:

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder.Services.AddMauiBlazorBridgeAsScoped();
}
```

---

## Layout Setup  

To integrate MAUI components within BlazorWebView, modify your `MainPage.xaml`.  
Wrap the **BlazorWebView** inside a `Grid`, and include an **AbsoluteLayout** (`OverlayContainer`) to host native elements:

```xml
<Grid>
    <!-- Blazor WebView -->
    <BlazorWebView x:Name="blazorWebView"
                   HostPage="wwwroot/index.html">
        <BlazorWebView.RootComponents>
            <RootComponent Selector="#app" ComponentType="{x:Type local:Routes}" />
        </BlazorWebView.RootComponents>
    </BlazorWebView>

    <!-- Overlay for native MAUI components -->
    <AbsoluteLayout x:Name="OverlayContainer" BackgroundColor="Transparent" />
</Grid>
```

This setup ensures that MAUI-native elements overlay correctly within your BlazorWebView.

---

## Usage  

To bridge MAUI elements into Blazor, use either:  

- **`MauiBlazorTypedBridge<T>`** (Typed binding)
- **`MauiBlazorGenericBridge`** (Generic binding)

### Example: Embedding a `MauiLabel` in Blazor  

```razor
@using Microsoft.Maui.Controls

@implements IAsyncDisposable

<MauiBlazorTypedBridge @ref="_bridge" TComponent="Label" Component="_label" />

@code {
    Label? _label;
    MauiBlazorTypedBridge<Label>? _bridge;

    protected override void OnInitialized()
    {
        _label = new Label
        { 
            Text = "This is a MAUI Label", 
            BackgroundColor = Colors.Transparent, 
            TextColor = Colors.Black 
        };
    }

    public async ValueTask DisposeAsync()
    {
        if (_bridge != null)
            await _bridge.DisposeAsync();
    }
}
```

The bridge observes its placeholder element in the browser and keeps the native view aligned beneath it. Dispose the bridge when the Blazor component is removed; disposal unregisters the callback, browser observers, native handlers, and overlay view.

The overlay sits above the web view. Do not set `InputTransparent="True"` when the native views must receive taps. Areas covered by the overlay can intercept input intended for the underlying web content, so keep native view bounds as small as practical.

`MauiBlazorGenericBridge` creates a parameterless `View` type and can apply common or dictionary-supplied public properties. Use the typed bridge when you already own the view instance or need native event handlers.
