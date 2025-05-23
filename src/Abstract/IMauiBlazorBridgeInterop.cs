﻿using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Maui.Blazor.Bridge.Abstract;

public interface IMauiBlazorBridgeInterop : IAsyncDisposable
{
    ValueTask Initialize(CancellationToken cancellationToken = default);

    ValueTask ObserveElementPosition(ElementReference reference, string elementId, CancellationToken cancellationToken = default);
}