namespace Soenneker.Maui.Blazor.Bridge.Demo;

public class MauiLabel : View
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(MauiLabel), null);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}