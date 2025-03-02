namespace Soenneker.Maui.Blazor.Bridge.Demo;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage()) { Title = "Soenneker.Maui.Blazor.Bridge.Demo" };
    }
}