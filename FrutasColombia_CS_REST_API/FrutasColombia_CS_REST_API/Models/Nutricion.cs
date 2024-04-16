using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Nutricion
    {
        [JsonPropertyName("azucares")]
        public double Azucares { get; set; } = 0d;

        [JsonPropertyName("carbohidratos")]
        public double Carbohidratos { get; set; } = 0d;

        [JsonPropertyName("grasas")]
        public double Grasas { get; set; } = 0d;

        [JsonPropertyName("proteinas")]
        public double Proteinas { get; set; } = 0d;
    }
}
