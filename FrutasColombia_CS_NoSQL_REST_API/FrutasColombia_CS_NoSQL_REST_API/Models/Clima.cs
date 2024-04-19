using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class Clima
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        [JsonPropertyName("nombre")]
        [BsonRepresentation(BsonType.String)]
        public string? Nombre { get; set; } = string.Empty;

        [BsonElement("altitud_minima")]
        [JsonPropertyName("altitud_minima")]
        [BsonRepresentation(BsonType.Int32)]
        public int Altitud_Minima { get; set; } = 0;

        [BsonElement("altitud_maxima")]
        [JsonPropertyName("altitud_maxima")]
        [BsonRepresentation(BsonType.Int32)]
        public int Altitud_Maxima { get; set; } = 0;
    }
}
