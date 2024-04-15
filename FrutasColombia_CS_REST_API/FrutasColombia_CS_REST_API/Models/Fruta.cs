using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Fruta
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("url_wikipedia")]
        public string? Url_Wikipedia { get; set; } = string.Empty;

        [JsonPropertyName("url_imagen")]
        public string? Url_Imagen { get; set; } = string.Empty;
    }
}
