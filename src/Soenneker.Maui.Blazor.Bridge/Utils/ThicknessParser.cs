using System;
using System.Globalization;
using Microsoft.Maui;

namespace Soenneker.Maui.Blazor.Bridge.Utils;

internal static class ThicknessParser
{
    public static Thickness Parse(ReadOnlySpan<char> s)
    {
        s = s.Trim();
        if (s.IsEmpty)
            return default;

        double a = 0, b = 0, c = 0, d = 0;
        int count = 0;

        while (!s.IsEmpty && count < 4)
        {
            int comma = s.IndexOf(',');
            ReadOnlySpan<char> part;

            if (comma < 0)
            {
                part = s;
                s = default;
            }
            else
            {
                part = s[..comma];
                s = s[(comma + 1)..];
            }

            part = part.Trim();

            if (!double.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out double val))
                val = 0;

            switch (count)
            {
                case 0: a = val; break;
                case 1: b = val; break;
                case 2: c = val; break;
                case 3: d = val; break;
            }

            count++;
        }

        return count switch
        {
            1 => new Thickness(a),
            2 => new Thickness(a, b),
            4 => new Thickness(a, b, c, d),
            _ => default
        };
    }
}