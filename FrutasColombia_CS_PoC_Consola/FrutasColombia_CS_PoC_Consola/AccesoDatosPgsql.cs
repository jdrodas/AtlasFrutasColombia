using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace FrutasColombia_CS_PoC_Consola
{
    public class AccesoDatosPgsql
    {
        public static string? ObtieneCadenaConexion()
        {
            //Parametrizamos el acceso al archivo de configuración appsettings.json
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration miConfiguracion = builder.Build();

            return miConfiguracion["ConnectionString:Pgsql"];
        }

        #region Frutas

        /// <summary>
        /// Obtiene la lista con los nombres de las frutas
        /// </summary>
        /// <returns>Lista con los nombres de las frutas</returns>
        public static List<string> ObtieneNombresFrutas()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            using IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion);
            string sentenciaSQL = "SELECT nombre FROM frutas ORDER BY nombre";
            var resultadoEstilos = cxnDB.Query<string>(sentenciaSQL, new DynamicParameters());

            return resultadoEstilos.AsList();
        }

        #endregion Frutas
    }
}
