using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Genero
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("familia")]
        public string? Familia { get; set; } = string.Empty;

        [JsonPropertyName("total_especies")]
        public int Total_Especies { get; set; } = 0;
    }
}
