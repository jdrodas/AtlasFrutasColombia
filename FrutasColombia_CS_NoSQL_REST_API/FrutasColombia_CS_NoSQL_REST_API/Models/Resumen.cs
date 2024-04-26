using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Resumen
    {
        [JsonPropertyName("frutas")]
        public long Frutas { get; set; } = 0;

        //[JsonPropertyName("departamentos")]
        //public int Departamentos { get; set; } = 0;

        //[JsonPropertyName("municipios")]
        //public int Municipios { get; set; } = 0;

        [JsonPropertyName("taxonomia_reinos")]
        public long Taxonomia_Reinos { get; set; } = 0;

        [JsonPropertyName("taxonomia_divisiones")]
        public long Taxonomia_Divisiones { get; set; } = 0;

        [JsonPropertyName("taxonomia_clases")]
        public long Taxonomia_Clases { get; set; } = 0;

        [JsonPropertyName("taxonomia_ordenes")]
        public long Taxonomia_Ordenes { get; set; } = 0;

        [JsonPropertyName("taxonomia_familias")]
        public long Taxonomia_Familias { get; set; } = 0;

        [JsonPropertyName("taxonomia_generos")]
        public long Taxonomia_Generos { get; set; } = 0;

        [JsonPropertyName("taxonomias_especies")]
        public long Taxonomia_Especies { get; set; } = 0;

        [JsonPropertyName("epocas")]
        public long Epocas { get; set; } = 0;

        [JsonPropertyName("climas")]
        public long Climas { get; set; } = 0;
    }
}
