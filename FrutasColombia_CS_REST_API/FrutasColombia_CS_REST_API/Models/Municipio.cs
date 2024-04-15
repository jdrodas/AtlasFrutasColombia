using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Municipio
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("departamento")]
        public string? Departamento { get; set; } = string.Empty;
    }
}
