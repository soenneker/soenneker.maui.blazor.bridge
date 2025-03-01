﻿using Microsoft.AspNetCore.Components;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Soenneker.Blazor.CallbackRegistry.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Maui.Blazor.Bridge.Abstract;
using Soenneker.Maui.Blazor.Bridge.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Soenneker.Maui.Blazor.Bridge;

public class MauiBlazorBridgeComponent : ComponentBase, IAsyncDisposable
{
    public readonly string ElementId = Guid.NewGuid().ToString();

    [Parameter] public Type ComponentType { get; set; } = null!;
    [Parameter] public string? Text { get; set; }
    [Parameter] public string? TextColor { get; set; }
    [Parameter] public string? BackgroundColor { get; set; }
    [Parameter] public int? FontSize { get; set; }
    [Parameter] public int? Padding { get; set; }
    [Parameter] public int? Margin { get; set; }
    [Parameter] public int? Width { get; set; }
    [Parameter] public int? Height { get; set; }
    [Parameter] public double? Opacity { get; set; }
    [Parameter] public double? Rotation { get; set; }
    [Parameter] public double? Scale { get; set; }
    [Parameter] public LayoutOptions? HorizontalOptions { get; set; }
    [Parameter] public LayoutOptions? VerticalOptions { get; set; }
    [Parameter] public double? X { get; set; } // Absolute positioning X
    [Parameter] public double? Y { get; set; } // Absolute positioning Y
    [Parameter] public bool UseVerticalStack { get; set; } = false;
    [Parameter] public bool UseHorizontalStack { get; set; } = false;
    [Parameter] public Dictionary<string, object?> Properties { get; set; } = new();

    [Inject] 
    private IBlazorCallbackRegistry _blazorCallbackRegistry { get; set; } = null!;

    [Inject]
    public IMauiBridgeInterop MauiBridgeInterop { get; set; } = null!;

    private View? _overlayView; // The container for the native MAUI component

    protected override async Task OnInitializedAsync()
    {
        await MauiBridgeInterop.Init().NoSync();

        await _blazorCallbackRegistry.Register<ElementPositionEventArgs>(ElementId, async result =>
        {
            await OnElementPositionReported(null, result);
        }).NoSync();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Window? window = Application.Current?.Windows.FirstOrDefault();
            if (window?.Page is ContentPage contentPage)
            {
                // Preserve existing content (e.g., the BlazorWebView)
                View? existingContent = contentPage.Content;

                // Create an AbsoluteLayout to hold both the BlazorWebView and our overlay.
                var absoluteLayout = new AbsoluteLayout();

                if (existingContent != null)
                {
                    AbsoluteLayout.SetLayoutFlags(existingContent, AbsoluteLayoutFlags.All);
                    AbsoluteLayout.SetLayoutBounds(existingContent, new Rect(0, 0, 1, 1));
                    absoluteLayout.Children.Add(existingContent);
                }

                // Create the native MAUI component (e.g., MauiLabel)
                View? mauiComponent = CreateMauiComponent();

                if (mauiComponent == null) 
                    return;

                ApplyCommonProperties(mauiComponent);
                ApplyExtraProperties(mauiComponent);

                // Wrap the component in an auto-sizing container
                Layout wrappedComponent = WrapInLayout(mauiComponent);

                // Initially position the overlay (fallback position)
                _overlayView = wrappedComponent;
                AbsoluteLayout.SetLayoutFlags(_overlayView, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(_overlayView, new Rect(0.5, 0.8, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                absoluteLayout.Children.Add(_overlayView);

                contentPage.Content = absoluteLayout;
            }
        });
    }

    private async Task OnElementPositionReported(object? sender, ElementPositionEventArgs e)
    {
        // Get the Maui page and its height (in MAUI device-independent units)
        Page? page = Application.Current?.MainPage;

        if (page == null || page.Height <= 0)
            return;

        // Compute a conversion factor:
        // e.ViewportHeight is the viewport height in CSS pixels from JS,
        // and page.Height is the Maui page height.
        double conversionFactor = page.Height / e.ViewportHeight;

        // Calculate the desired overlay Y position in CSS pixels.
        // For example, take the bottom of the element and add a dynamic offset
        // (here, we add half the element's height).
        double dynamicOffsetCss = e.Height * 0.5;
        double overlayYCss = e.Top + e.Height + dynamicOffsetCss;

        // Convert the CSS pixel value to Maui units using the conversion factor.
        double overlayYMaui = overlayYCss * conversionFactor;

        // Convert to a proportional value (0 to 1) relative to the Maui page height.
        double yProportion = overlayYMaui / page.Height;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_overlayView != null)
            {
                // Position the overlay using proportional coordinates.
                AbsoluteLayout.SetLayoutFlags(_overlayView, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(_overlayView, new Rect(0.5, yProportion, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            }
        });
    }


    private View? CreateMauiComponent()
    {
        if (!typeof(View).IsAssignableFrom(ComponentType))
        {
            Console.WriteLine($"Unsupported or non-view MAUI component: {ComponentType}");
            return null;
        }
        return (View)Activator.CreateInstance(ComponentType)!;
    }

    private void ApplyCommonProperties(View mauiComponent)
    {
        if (!string.IsNullOrEmpty(BackgroundColor))
            mauiComponent.BackgroundColor = Color.FromArgb(BackgroundColor);

        if (Width.HasValue) mauiComponent.WidthRequest = Width.Value;
        if (Height.HasValue) mauiComponent.HeightRequest = Height.Value;
        if (Margin.HasValue) mauiComponent.Margin = new Thickness(Margin.Value);
        if (Opacity.HasValue) mauiComponent.Opacity = Opacity.Value;
        if (Rotation.HasValue) mauiComponent.Rotation = Rotation.Value;
        if (Scale.HasValue) mauiComponent.Scale = Scale.Value;

        if (HorizontalOptions.HasValue)
            mauiComponent.HorizontalOptions = HorizontalOptions.Value;
        if (VerticalOptions.HasValue)
            mauiComponent.VerticalOptions = VerticalOptions.Value;
        if (X.HasValue) mauiComponent.TranslationX = X.Value;
        if (Y.HasValue) mauiComponent.TranslationY = Y.Value;

        SetComponentSpecificProperties(mauiComponent);
    }

    private void ApplyExtraProperties(View mauiComponent)
    {
        foreach (KeyValuePair<string, object> prop in Properties)
        {
            PropertyInfo? propertyInfo = mauiComponent.GetType().GetProperty(prop.Key);
            if (propertyInfo != null && prop.Value != null)
            {
                object? convertedValue = ConvertValue(propertyInfo.PropertyType, prop.Value);
                propertyInfo.SetValue(mauiComponent, convertedValue);
            }
        }
    }

    private void SetComponentSpecificProperties(View mauiComponent)
    {
        PropertyInfo? textProperty = mauiComponent.GetType().GetProperty("Text");
        if (textProperty != null && !string.IsNullOrEmpty(Text))
            textProperty.SetValue(mauiComponent, Text);

        PropertyInfo? textColorProperty = mauiComponent.GetType().GetProperty("TextColor");
        if (textColorProperty != null && !string.IsNullOrEmpty(TextColor))
            textColorProperty.SetValue(mauiComponent, Color.FromArgb(TextColor));

        PropertyInfo? fontSizeProperty = mauiComponent.GetType().GetProperty("FontSize");
        if (fontSizeProperty != null && FontSize.HasValue)
            fontSizeProperty.SetValue(mauiComponent, FontSize.Value);

        PropertyInfo? paddingProperty = mauiComponent.GetType().GetProperty("Padding");
        if (paddingProperty != null && Padding.HasValue)
            paddingProperty.SetValue(mauiComponent, new Thickness(Padding.Value));
    }

    private object? ConvertValue(Type targetType, object value)
    {
        if (targetType == typeof(Color) && value is string colorString)
            return Color.FromArgb(colorString);
        if (targetType == typeof(LayoutOptions) && value is string layoutString)
            return GetLayoutOptions(layoutString);
        if (targetType == typeof(bool) && value is string boolString)
            return bool.Parse(boolString);
        if (targetType == typeof(double) && value is string doubleString)
            return double.Parse(doubleString);
        if (targetType == typeof(int) && value is string intString)
            return int.Parse(intString);
        if (targetType == typeof(Thickness) && value is string thicknessString)
        {
            double[] values = thicknessString.Split(',').Select(double.Parse).ToArray();
            return values.Length switch
            {
                1 => new Thickness(values[0]),
                2 => new Thickness(values[0], values[1]),
                4 => new Thickness(values[0], values[1], values[2], values[3]),
                _ => new Thickness(0)
            };
        }
        return value;
    }

    private static LayoutOptions GetLayoutOptions(string alignment) =>
        alignment.ToLower() switch
        {
            "start" => LayoutOptions.Start,
            "center" => LayoutOptions.Center,
            "end" => LayoutOptions.End,
            "fill" => LayoutOptions.Fill,
            _ => LayoutOptions.Center
        };

    private Layout WrapInLayout(View mauiComponent)
    {
        if (UseVerticalStack)
        {
            var stackLayout = new VerticalStackLayout
            {
                VerticalOptions = VerticalOptions ?? LayoutOptions.Start,
                HorizontalOptions = HorizontalOptions ?? LayoutOptions.Start
            };
            stackLayout.Children.Add(mauiComponent);
            return stackLayout;
        }

        if (UseHorizontalStack)
        {
            var stackLayout = new HorizontalStackLayout
            {
                VerticalOptions = VerticalOptions ?? LayoutOptions.Start,
                HorizontalOptions = HorizontalOptions ?? LayoutOptions.Start
            };
            stackLayout.Children.Add(mauiComponent);
            return stackLayout;
        }

        var grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Auto }
            },
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start
        };
        grid.Children.Add(mauiComponent);
        return grid;
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        _blazorCallbackRegistry.Unregister(ElementId);
    }
}