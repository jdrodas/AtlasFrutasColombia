using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Clase
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("division")]
        public string? Division { get; set; } = string.Empty;
    }
}
