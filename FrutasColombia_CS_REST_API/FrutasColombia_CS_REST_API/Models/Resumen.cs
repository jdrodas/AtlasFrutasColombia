using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Resumen
    {
        [JsonPropertyName("frutas")]
        public int Frutas { get; set; } = 0;

        [JsonPropertyName("departamentos")]
        public int Departamentos { get; set; } = 0;

        [JsonPropertyName("municipios")]
        public int Municipios { get; set; } = 0;

        [JsonPropertyName("taxonomia_reinos")]
        public int Taxonomia_Reinos { get; set; } = 0;

        [JsonPropertyName("taxonomia_divisiones")]
        public int Taxonomia_Divisiones { get; set; } = 0;

        [JsonPropertyName("taxonomia_clases")]
        public int Taxonomia_Clases { get; set; } = 0;

        [JsonPropertyName("taxonomia_ordenes")]
        public int Taxonomia_Ordenes { get; set; } = 0;

        [JsonPropertyName("taxonomia_familias")]
        public int Taxonomia_Familias { get; set; } = 0;

        [JsonPropertyName("taxonomia_generos")]
        public int Taxonomia_Generos { get; set; } = 0;

        [JsonPropertyName("taxonomias_especies")]
        public int Taxonomia_Especies { get; set; } = 0;

        [JsonPropertyName("epocas")]
        public int Epocas { get; set; } = 0;

        [JsonPropertyName("climas")]
        public int Climas { get; set; } = 0;
    }
}
