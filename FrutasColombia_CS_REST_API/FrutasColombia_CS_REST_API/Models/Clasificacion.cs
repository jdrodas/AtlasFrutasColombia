using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Clasificacion
    {
        [JsonPropertyName("reino_id")]
        public int Reino_Id { get; set; } = 0;

        [JsonPropertyName("reino_nombre")]
        public string? Reino_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("division_id")]
        public int Division_Id { get; set; } = 0;

        [JsonPropertyName("division_nombre")]
        public string? Division_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("clase_id")]
        public int Clase_Id { get; set; } = 0;

        [JsonPropertyName("clase_nombre")]
        public string? Clase_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("orden_id")]
        public int Orden_Id { get; set; } = 0;

        [JsonPropertyName("orden_nombre")]
        public string? Orden_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("familia_id")]
        public int Familia_Id { get; set; } = 0;

        [JsonPropertyName("familia_nombre")]
        public string? Familia_Nombre { get; set; } = string.Empty;


        [JsonPropertyName("genero_id")]
        public int Genero_Id { get; set; } = 0;

        [JsonPropertyName("genero_nombre")]
        public string? Genero_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("especie_id")]
        public int Especie_Id { get; set; } = 0;

        [JsonPropertyName("especie_nombre")]
        public string? Especie_Nombre { get; set; } = string.Empty;
    }
}
