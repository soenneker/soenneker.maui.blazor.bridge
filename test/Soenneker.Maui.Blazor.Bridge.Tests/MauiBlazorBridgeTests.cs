using Soenneker.Tests.HostedUnit;

namespace Soenneker.Maui.Blazor.Bridge.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class MauiBlazorBridgeTests : HostedUnitTest
{
    public MauiBlazorBridgeTests(Host host) : base(host)
    {

    }

    [Test]
    public void Default()
    {

    }
}
