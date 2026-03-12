using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Soenneker.Maui.Blazor.Bridge.Utils;

internal static class MauiHandlerUtil
{
    public static void ForceHandlerIfNeeded(View view, IMauiContext mauiContext)
    {
        if (view.Handler is not null)
            return;

        IElementHandler? handler = mauiContext.Handlers.GetHandler(view.GetType());
        if (handler is IViewHandler viewHandler)
        {
            viewHandler.SetMauiContext(mauiContext);
            viewHandler.SetVirtualView(view);
            view.Handler = viewHandler;
        }
    }
}