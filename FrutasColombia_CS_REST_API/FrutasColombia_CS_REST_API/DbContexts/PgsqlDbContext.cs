using Npgsql;
using System.Data;

namespace FrutasColombia_CS_REST_API.DbContexts
{
    public class PgsqlDbContext
    {
        private readonly string cadenaConexion;
        public PgsqlDbContext(IConfiguration unaConfiguracion)
        {
            cadenaConexion = unaConfiguracion.GetConnectionString("FrutasPL")!;
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(cadenaConexion);
        }
    }
}