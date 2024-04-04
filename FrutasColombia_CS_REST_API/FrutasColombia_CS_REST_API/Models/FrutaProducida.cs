using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class FrutaProducida : Fruta
    {
        public List<Produccion>? Produccion { get; set; } = null;
    }
}
