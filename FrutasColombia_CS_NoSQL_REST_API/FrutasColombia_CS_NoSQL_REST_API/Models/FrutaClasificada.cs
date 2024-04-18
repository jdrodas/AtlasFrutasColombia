using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class FrutaClasificada : Fruta
    {
        [JsonPropertyName("taxonomia")]
        public Taxonomia? Taxonomia { get; set; } = null;
    }
}
