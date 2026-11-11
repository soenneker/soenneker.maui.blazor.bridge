namespace Soenneker.Maui.Blazor.Bridge.Demo;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
#if !DEBUG
        // Cache bundled assets; keep HTML and data responses on the default no-store policy.
        blazorWebView.StaticContentCacheControlProvider = static request =>
            request.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) ||
            request.ContentType.StartsWith("font/", StringComparison.OrdinalIgnoreCase) ||
            request.ContentType is "text/css" or "text/javascript" or "application/javascript" or "application/font-woff"
                ? "private, max-age=86400"
                : null;
#endif
    }
}
