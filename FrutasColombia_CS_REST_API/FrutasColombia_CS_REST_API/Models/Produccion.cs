using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Produccion
    {
        [JsonPropertyName("epoca")]
        public string? Epoca { get; set; } = string.Empty;

        [JsonPropertyName("clima")]
        public string? Clima { get; set; } = string.Empty;

        [JsonPropertyName("municipio")]
        public string? Municipio { get; set; } = string.Empty;

        [JsonPropertyName("departamento")]
        public string? Departamento { get; set; } = string.Empty;
    }
}
