using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Produccion
    {
        [JsonPropertyName("epoca")]
        public string? Epoca { get; set; } = string.Empty;

        [JsonPropertyName("clima")]
        public string? Clima { get; set; } = string.Empty;

        [JsonPropertyName("municipio")]
        public string? Municipio { get; set; } = string.Empty;

        [JsonPropertyName("departamento")]
        public string? Departamento { get; set; } = string.Empty;

        [JsonPropertyName("mes_inicio")]
        public int Mes_Inicio { get; set; } = 0;

        [JsonPropertyName("mes_final")]
        public int Mes_Final { get; set; } = 0;

        public override string ToString()
        {
            string informacion = $"Epoca: {Epoca}, " +
                $"Clima: {Clima}, " +
                $"Municipio: {Municipio}, " +
                $"Departamento: {Departamento}," +
                $"Mes Inicio: {Mes_Inicio}, " +
                $"Mes Final: {Mes_Final}";
            
            return informacion;
        }
    }
}
