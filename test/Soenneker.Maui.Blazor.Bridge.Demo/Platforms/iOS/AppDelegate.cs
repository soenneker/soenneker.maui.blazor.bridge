﻿using Foundation;

namespace Soenneker.Maui.Blazor.Bridge.Demo;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}