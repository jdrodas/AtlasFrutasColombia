using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Nutricion
    {
        [JsonPropertyName("azucares")]
        public string? Azucares { get; set; } = string.Empty;

        [JsonPropertyName("carbohidratos")]
        public string? Carbohidratos { get; set; } = string.Empty;

        [JsonPropertyName("grasas")]
        public string? Grasas { get; set; } = string.Empty;

        [JsonPropertyName("proteinas")]
        public string? Proteinas { get; set; } = string.Empty;
    }
}
