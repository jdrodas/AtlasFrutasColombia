using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Clasificacion
    {
        [JsonPropertyName("reino_id")]
        public string? Reino_Id { get; set; } = string?.Empty;

        [JsonPropertyName("reino_nombre")]
        public string? Reino_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("division_id")]
        public string? Division_Id { get; set; } = string?.Empty;

        [JsonPropertyName("division_nombre")]
        public string? Division_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("clase_id")]
        public string? Clase_Id { get; set; } = string?.Empty;

        [JsonPropertyName("clase_nombre")]
        public string? Clase_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("orden_id")]
        public string? Orden_Id { get; set; } = string?.Empty;

        [JsonPropertyName("orden_nombre")]
        public string? Orden_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("familia_id")]
        public string? Familia_Id { get; set; } = string?.Empty;

        [JsonPropertyName("familia_nombre")]
        public string? Familia_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("genero_id")]
        public string? Genero_Id { get; set; } = string?.Empty;

        [JsonPropertyName("genero_nombre")]
        public string? Genero_Nombre { get; set; } = string.Empty;

        [JsonPropertyName("especie_id")]
        public string? Especie_Id { get; set; } = string?.Empty;

        [JsonPropertyName("especie_nombre")]
        public string? Especie_Nombre { get; set; } = string.Empty;
    }
}
