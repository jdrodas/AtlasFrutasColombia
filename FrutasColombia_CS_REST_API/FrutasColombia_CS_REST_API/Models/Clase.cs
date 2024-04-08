using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Clase
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("division")]
        public string? Division { get; set; } = string.Empty;
    }
}
