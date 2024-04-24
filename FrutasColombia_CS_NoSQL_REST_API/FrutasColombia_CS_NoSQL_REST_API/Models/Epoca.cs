using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Epoca
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        [JsonPropertyName("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? Nombre { get; set; } = string.Empty;

        [BsonElement("mes_inicio")]
        [JsonPropertyName("mes_inicio")]
        [BsonRepresentation(BsonType.Int32)]
        public int Mes_Inicio { get; set; } = 0;


        [BsonElement("mes_final")]
        [JsonPropertyName("mes_final")]
        [BsonRepresentation(BsonType.Int32)]
        public int Mes_Final { get; set; } = 0;
    }
}
