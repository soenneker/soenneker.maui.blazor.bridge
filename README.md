# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Maui.Blazor.Bridge

[![](https://img.shields.io/nuget/v/soenneker.maui.blazor.bridge.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.maui.blazor.bridge/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.maui.blazor.bridge/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.maui.blazor.bridge/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.maui.blazor.bridge.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.maui.blazor.bridge/)

### Effortlessly integrate MAUI components within BlazorWebView, enabling seamless interaction between Blazor and native MAUI UI elements.


---

## 🚀 Features  

✅ Embed MAUI components directly inside **BlazorWebView** like HTML elements.  
✅ Maintain a structured overlay system for native elements.  
✅ Provides **typed** and **generic** bridges for flexible component integration.  

---

## 📦 Installation  

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

## 🛠️ Layout Setup  

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
    <AbsoluteLayout x:Name="OverlayContainer" BackgroundColor="Transparent" InputTransparent="True" />
</Grid>
```

This setup ensures that MAUI-native elements overlay correctly within your BlazorWebView.

---

## ⚡ Usage  

To bridge MAUI elements into Blazor, use either:  

- **`MauiBlazorTypedBridge<T>`** (Typed binding)
- **`MauiBlazorGenericBridge`** (Generic binding)

### 🏗️ Example: Embedding a `MauiLabel` in Blazor  

```razor
@page "/"

@implements IAsyncDisposable

<MauiBlazorTypedBridge @ref="_bridge" TComponent="MauiLabel" Component="_label"></MauiBlazorTypedBridge>

@code {
    MauiLabel? _label;
    MauiBlazorTypedBridge<MauiLabel>? _bridge;

    protected override void OnInitialized()
    {
        _label = new MauiLabel 
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

This example adds a **MauiLabel** component inside a Blazor page, allowing it to function within the BlazorWebView.