using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class FrutaDetallada : Fruta
    {
        [JsonPropertyName("produccion")]
        public List<Produccion>? Produccion { get; set; } = null;

        [JsonPropertyName("taxonomia")]
        public Taxonomia? Taxonomia { get; set; } = null;

        [JsonPropertyName("nutricion")]
        public Nutricion? Nutricion { get; set; } = null;
    }
}
