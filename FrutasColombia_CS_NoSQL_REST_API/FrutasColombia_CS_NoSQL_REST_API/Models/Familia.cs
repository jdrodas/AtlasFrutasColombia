using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Familia
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string?.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("orden")]
        public string? Orden { get; set; } = string.Empty;
    }
}
