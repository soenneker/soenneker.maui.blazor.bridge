namespace Soenneker.Maui.Blazor.Bridge.EventArgs;

public class ElementPositionEventArgs : System.EventArgs
{
    public double Top { get; }
    public double Left { get; }
    public double Width { get; }
    public double Height { get; }
    public double ViewportHeight { get; } // in CSS pixels

    public ElementPositionEventArgs(double top, double left, double width, double height, double viewportHeight)
    {
        Top = top;
        Left = left;
        Width = width;
        Height = height;
        ViewportHeight = viewportHeight;
    }
}