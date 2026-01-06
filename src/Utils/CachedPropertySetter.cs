using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Soenneker.Maui.Blazor.Bridge.Utils;

internal static class CachedPropertySetter
{
    private static readonly ConcurrentDictionary<(Type type, string name), Action<object, object?>?> _cache = new();

    public static void TrySet(object target, string propertyName, object? value)
    {
        if (target is null || propertyName is null || value is null)
            return;

        (Type, string propertyName) key = (target.GetType(), propertyName);

        Action<object, object?>? setter = _cache.GetOrAdd(key, static k =>
        {
            PropertyInfo? pi = k.type.GetProperty(k.name, BindingFlags.Instance | BindingFlags.Public);
            if (pi is null || !pi.CanWrite)
                return null;

            ParameterExpression objParam = Expression.Parameter(typeof(object), "obj");
            ParameterExpression valParam = Expression.Parameter(typeof(object), "val");

            UnaryExpression castObj = Expression.Convert(objParam, k.type);
            MemberExpression propExpr = Expression.Property(castObj, pi);

            Type propType = pi.PropertyType;
            Expression convertedVal = BuildConversionExpression(propType, valParam);

            BinaryExpression assign = Expression.Assign(propExpr, convertedVal);
            return Expression.Lambda<Action<object, object?>>(assign, objParam, valParam).Compile();
        });

        if (setter is null)
            return;

        try
        {
            setter(target, value);
        }
        catch
        {
            // swallow invalid assignments from user dictionaries
        }
    }

    private static Expression BuildConversionExpression(Type propType, ParameterExpression valParam)
    {
        if (propType == typeof(Color))
            return Expression.Call(typeof(CachedPropertySetter), nameof(ConvertToColor), null, valParam);

        if (propType == typeof(LayoutOptions))
            return Expression.Call(typeof(CachedPropertySetter), nameof(ConvertToLayoutOptions), null, valParam);

        if (propType == typeof(Thickness))
            return Expression.Call(typeof(CachedPropertySetter), nameof(ConvertToThickness), null, valParam);

        if (propType == typeof(bool))
            return Expression.Call(typeof(CachedPropertySetter), nameof(ConvertToBool), null, valParam);

        if (propType == typeof(double))
            return Expression.Call(typeof(CachedPropertySetter), nameof(ConvertToDouble), null, valParam);

        if (propType == typeof(int))
            return Expression.Call(typeof(CachedPropertySetter), nameof(ConvertToInt), null, valParam);

        return Expression.Convert(valParam, propType);
    }

    private static Color ConvertToColor(object? o)
    {
        if (o is Color c) return c;
        if (o is string s) return Color.FromArgb(s);
        return default;
    }

    private static LayoutOptions ConvertToLayoutOptions(object? o)
    {
        if (o is LayoutOptions lo) return lo;
        if (o is string s) return LayoutOptionsParser.Parse(s);
        return LayoutOptions.Center;
    }

    private static Thickness ConvertToThickness(object? o)
    {
        if (o is Thickness t) return t;
        if (o is string s) return ThicknessParser.Parse(s.AsSpan());
        if (o is int i) return new Thickness(i);
        if (o is double d) return new Thickness(d);
        return default;
    }

    private static bool ConvertToBool(object? o)
    {
        if (o is bool b) return b;
        if (o is string s) return bool.Parse(s);
        return false;
    }

    private static double ConvertToDouble(object? o)
    {
        if (o is double d) return d;
        if (o is float f) return f;
        if (o is int i) return i;
        if (o is string s) return double.Parse(s, CultureInfo.InvariantCulture);
        return 0;
    }

    private static int ConvertToInt(object? o)
    {
        if (o is int i) return i;
        if (o is string s) return int.Parse(s, CultureInfo.InvariantCulture);
        if (o is double d) return (int)d;
        return 0;
    }
}
