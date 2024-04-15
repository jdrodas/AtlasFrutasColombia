using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Clima
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("altitud_minima")]
        public int Altitud_Minima { get; set; } = 0;

        [JsonPropertyName("altitud_maxima")]
        public int Altitud_Maxima { get; set; } = 0;
    }
}