using Android.Content;
using Android.Widget;
using Microsoft.Maui.Handlers;
using Color = Android.Graphics.Color;

namespace Soenneker.Maui.Blazor.Bridge.Demo.Platforms.Android;

public partial class MauiLabelHandler : ViewHandler<MauiLabel, TextView>
{
    public static IPropertyMapper<MauiLabel, MauiLabelHandler> Mapper = new PropertyMapper<MauiLabel, MauiLabelHandler>(ViewMapper)
    {
        [nameof(MauiLabel.Text)] = (handler, view) => handler.UpdateText()
    };

    public MauiLabelHandler() : base(Mapper)
    {
    }

    protected override TextView CreatePlatformView()
    {
        Context context = MauiContext?.Context ?? throw new InvalidOperationException("MauiContext is null");
        var textView = new TextView(context);
        textView.SetTextColor(Color.Black);
        textView.SetBackgroundColor(Color.LightGray);
        return textView;
    }

    public void SetText(string text)
    {
        if (PlatformView != null)
            PlatformView.Text = text;
    }

    private void UpdateText()
    {
        SetText(VirtualView.Text);
    }
}