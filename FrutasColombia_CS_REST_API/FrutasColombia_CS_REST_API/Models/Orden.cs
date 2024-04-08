using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Orden
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("clase")]
        public string? Clase { get; set; } = string.Empty;
    }
}
