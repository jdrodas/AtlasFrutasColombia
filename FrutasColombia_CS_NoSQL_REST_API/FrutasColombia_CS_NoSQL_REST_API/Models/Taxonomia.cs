using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Taxonomia
    {
        [JsonPropertyName("reino")]
        public string? Reino { get; set; } = string.Empty;

        [JsonPropertyName("division")]
        public string? Division { get; set; } = string.Empty;

        [JsonPropertyName("clase")]
        public string? Clase { get; set; } = string.Empty;

        [JsonPropertyName("orden")]
        public string? Orden { get; set; } = string.Empty;

        [JsonPropertyName("familia")]
        public string? Familia { get; set; } = string.Empty;

        [JsonPropertyName("genero")]
        public string? Genero { get; set; } = string.Empty;

        [JsonPropertyName("especie")]
        public string? Especie { get; set; } = string.Empty;
    }
}
