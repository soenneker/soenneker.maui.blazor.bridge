using System.Text.Json.Serialization;

namespace Soenneker.Maui.Blazor.Bridge.Dtos;

public record ElementPositionDto
{
    [JsonPropertyName("top")]
    public double Top { get; set;  }

    [JsonPropertyName("left")]
    public double Left { get; set; }

    [JsonPropertyName("width")]
    public double Width { get; set; }

    [JsonPropertyName("height")]
    public double Height { get; set; }

    /// <summary>
    /// in css pixels
    /// </summary>
    [JsonPropertyName("viewportHeight")]
    public double ViewportHeight { get; set; } 
}