using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class FrutaNutritiva : Fruta
    {
        [JsonPropertyName("nutricion")]
        public Nutricion? Nutricion { get; set; } = null;
    }
}
