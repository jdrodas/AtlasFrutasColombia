using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Division
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string?.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("reino")]
        public string? Reino { get; set; } = string.Empty;
    }
}
