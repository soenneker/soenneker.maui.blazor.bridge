using System;
using Microsoft.Maui.Controls;

namespace Soenneker.Maui.Blazor.Bridge.Utils;

internal static class LayoutOptionsParser
{
    public static LayoutOptions Parse(string alignment)
    {
        if (string.Equals(alignment, "start", StringComparison.OrdinalIgnoreCase))
            return LayoutOptions.Start;
        if (string.Equals(alignment, "center", StringComparison.OrdinalIgnoreCase))
            return LayoutOptions.Center;
        if (string.Equals(alignment, "end", StringComparison.OrdinalIgnoreCase))
            return LayoutOptions.End;
        if (string.Equals(alignment, "fill", StringComparison.OrdinalIgnoreCase))
            return LayoutOptions.Fill;
        return LayoutOptions.Center;
    }
}