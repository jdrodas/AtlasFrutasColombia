﻿using System.Text.Json.Serialization;

namespace FrutasColombia_CS_REST_API.Models
{
    public class FrutaDetallada : Fruta
    {
        public List<Produccion>? Produccion { get; set; } = null;

        public Taxonomia? Taxonomia { get; set; } = null;

        public Nutricion? Nutricion { get; set; } = null;
    }
}
