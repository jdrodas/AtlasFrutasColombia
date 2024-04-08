using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Familia
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("orden")]
        public string? Orden { get; set; } = string.Empty;
    }
}
