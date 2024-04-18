using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Departamento
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;
    }
}
