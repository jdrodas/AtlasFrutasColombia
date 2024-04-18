using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class GeneroDetallado : Genero
    {
        [JsonPropertyName("especies")]
        public List<Especie>? Especies { get; set; } = null;
    }
}
