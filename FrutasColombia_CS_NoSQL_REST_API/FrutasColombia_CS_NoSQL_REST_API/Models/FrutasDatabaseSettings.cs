namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class FrutasDatabaseSettings
    {
        public string DatabaseName { get; set; } = null!;
        public string ColeccionFrutas { get; set; } = null!;

        public string ColeccionClimas { get; set; } = null!;

        public string ColeccionTaxonomias { get; set; } = null!;

        public string ColeccionEpocas { get; set; } = null!;

        public FrutasDatabaseSettings(IConfiguration unaConfiguracion)
        {
            var configuracion = unaConfiguracion.GetSection("FrutasDatabaseSettings");

            DatabaseName = configuracion.GetSection("DatabaseName").Value!;
            ColeccionFrutas = configuracion.GetSection("ColeccionFrutas").Value!;
            ColeccionClimas = configuracion.GetSection("ColeccionClimas").Value!;
            ColeccionTaxonomias = configuracion.GetSection("ColeccionTaxonomias").Value!;
            ColeccionEpocas = configuracion.GetSection("ColeccionEpocas").Value!;
        }
    }
}