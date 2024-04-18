using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace FrutasColombia_CS_PoC_Consola
{
    public class Fruta
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? ObjectId { get; set; }

        //[JsonPropertyName("id")]
        //public Guid Id { get; set; } = Guid.Empty;
        
        [BsonElement("nombre")]
        [BsonRepresentation(BsonType.String)]
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = String.Empty;

        [BsonElement("url_wikipedia")]
        [BsonRepresentation(BsonType.String)]
        [JsonPropertyName("url_wikipedia")]
        public string? Url_Wikipedia { get; set; } = String.Empty;
        
        [BsonElement("url_imagen")]
        [BsonRepresentation(BsonType.String)]
        [JsonPropertyName("url_imagen")]
        public string? Url_Imagen { get; set; } = String.Empty;
    }
}