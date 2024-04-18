using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class FrutaProducida : Fruta
    {
        [JsonPropertyName("produccion")]
        public List<Produccion>? Produccion { get; set; } = null;
    }
}
