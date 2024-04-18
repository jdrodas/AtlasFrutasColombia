namespace FrutasColombia_CS_NoSQL_REST_API.Models
{
    public class FrutasDatabaseSettings
    {
        public string DatabaseName { get; set; } = null!;
        public string ColeccionFrutas { get; set; } = null!;

        public FrutasDatabaseSettings(IConfiguration unaConfiguracion)
        {
            var configuracion = unaConfiguracion.GetSection("FrutasDatabaseSettings");

            DatabaseName = configuracion.GetSection("DatabaseName").Value!;
            ColeccionFrutas = configuracion.GetSection("ColeccionFrutas").Value!;

        }
    }
}