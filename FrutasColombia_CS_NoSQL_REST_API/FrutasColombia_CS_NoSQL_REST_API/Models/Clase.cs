using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Clase
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string?.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("division")]
        public string? Division { get; set; } = string.Empty;
    }
}
