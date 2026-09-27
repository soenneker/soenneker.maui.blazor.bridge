using System.Text.Json.Serialization;

namespace Soenneker.Maui.Blazor.Bridge.Dtos;

[JsonSerializable(typeof(ElementPositionDto))]
internal partial class BridgeJsonContext : JsonSerializerContext
{
}
