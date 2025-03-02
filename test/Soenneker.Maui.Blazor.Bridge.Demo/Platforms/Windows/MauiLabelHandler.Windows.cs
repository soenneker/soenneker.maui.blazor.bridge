using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Controls;

namespace Soenneker.Maui.Blazor.Bridge.Demo;

public partial class MauiLabelHandler : ViewHandler<MauiLabel, TextBlock>
{
    public static IPropertyMapper<MauiLabel, MauiLabelHandler> Mapper = new PropertyMapper<MauiLabel, MauiLabelHandler>(ViewMapper)
    {
        [nameof(MauiLabel.Text)] = (handler, view) => handler.UpdateText()
    };

    public MauiLabelHandler() : base(Mapper)
    {
    }

    protected override TextBlock CreatePlatformView()
    {
        return new TextBlock
        {
            Text = "Default Text",
            Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Black),
            FontSize = 16
        };
    }

    private void UpdateText()
    {
        if (PlatformView != null)
            PlatformView.Text = VirtualView.Text;
    }
}