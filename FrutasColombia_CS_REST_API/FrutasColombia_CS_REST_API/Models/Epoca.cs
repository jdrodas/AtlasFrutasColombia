using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Epoca
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [JsonPropertyName("mes_inicio")]
        public int Mes_Inicio { get; set; } = 0;

        [JsonPropertyName("mes_final")]
        public int Mes_Final { get; set; } = 0;
    }
}
