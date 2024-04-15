using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Division
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("reino")]
        public string? Reino { get; set; } = string.Empty;
    }
}
