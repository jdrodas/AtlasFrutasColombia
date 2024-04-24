using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Clasificacion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("reino")]
        [JsonPropertyName("reino")]
        [BsonRepresentation(BsonType.String)]
        public string? Reino { get; set; } = string.Empty;

        [BsonElement("division")]
        [JsonPropertyName("division")]
        [BsonRepresentation(BsonType.String)]
        public string? Division { get; set; } = string.Empty;

        [BsonElement("clase")]
        [JsonPropertyName("clase")]
        [BsonRepresentation(BsonType.String)]
        public string? Clase { get; set; } = string.Empty;

        [BsonElement("orden")]
        [JsonPropertyName("orden")]
        [BsonRepresentation(BsonType.String)]
        public string? Orden { get; set; } = string.Empty;

        [BsonElement("familia")]
        [JsonPropertyName("familia")]
        [BsonRepresentation(BsonType.String)]
        public string? Familia { get; set; } = string.Empty;

        [BsonElement("genero")]
        [JsonPropertyName("genero")]
        [BsonRepresentation(BsonType.String)]
        public string? Genero { get; set; } = string.Empty;

        [BsonElement("especie")]
        [JsonPropertyName("especie")]
        [BsonRepresentation(BsonType.String)]
        public string? Especie { get; set; } = string.Empty;
    }
}
