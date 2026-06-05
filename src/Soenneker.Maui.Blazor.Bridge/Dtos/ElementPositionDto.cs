using System.Text.Json.Serialization;

namespace Soenneker.Maui.Blazor.Bridge.Dtos;

/// <summary>
/// Represents the element position dto record.
/// </summary>
public record ElementPositionDto
{
    /// <summary>
    /// Gets or sets top.
    /// </summary>
    [JsonPropertyName("top")]
    public double Top { get; set;  }

    /// <summary>
    /// Gets or sets left.
    /// </summary>
    [JsonPropertyName("left")]
    public double Left { get; set; }

    /// <summary>
    /// Gets or sets width.
    /// </summary>
    [JsonPropertyName("width")]
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets height.
    /// </summary>
    [JsonPropertyName("height")]
    public double Height { get; set; }

    /// <summary>
    /// in css pixels
    /// </summary>
    [JsonPropertyName("viewportHeight")]
    public double ViewportHeight { get; set; } 
}