using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Municipio
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("departamento_id")]
        public string? Departamento_Id { get; set; } = string.Empty;
    }
}
