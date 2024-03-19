﻿using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class Departamento
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; } = string.Empty;
    }
}
