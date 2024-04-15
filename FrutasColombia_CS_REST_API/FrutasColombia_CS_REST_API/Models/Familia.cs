using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Familia
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("orden")]
        public string? Orden { get; set; } = string.Empty;
    }
}
