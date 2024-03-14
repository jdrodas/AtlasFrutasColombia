using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Resumen
    {
        [JsonPropertyName("frutas")]
        public int Frutas { get; set; } = 0;

        [JsonPropertyName("taxonomia_familias")]
        public int Taxonomia_Familias { get; set; } = 0;

        [JsonPropertyName("departamentos")]
        public int Departamentos { get; set; } = 0;
    }
}
