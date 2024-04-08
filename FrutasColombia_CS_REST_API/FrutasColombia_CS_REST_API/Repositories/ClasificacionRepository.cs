using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using System.Data;

namespace FrutasColombia_CS_REST_API.Repositories
{
    public class ClasificacionRepository(PgsqlDbContext unContexto) : IClasificacionRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Clasificacion>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT reino_nombre, division_nombre, " +
                "clase_nombre, orden_nombre, familia_nombre, genero_nombre, " +
                "especie_nombre, reino_id, division_id, clase_id, orden_id, " +
                "familia_id, genero_id, especie_id " +
                "FROM v_info_botanica " +
                "ORDER BY reino_nombre, division_nombre, clase_nombre, " +
                "orden_nombre, familia_nombre, genero_nombre, especie_nombre ";

            var resultadoClasificacion = await conexion
                .QueryAsync<Clasificacion>(sentenciaSQL, new DynamicParameters());

            return resultadoClasificacion;
        }

        public async Task<IEnumerable<Division>> GetAllDivisionsAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT reino_nombre reino, division_nombre nombre, division_id id " +
                "FROM v_info_botanica " +
                "ORDER BY reino_nombre, division_nombre";

            var resultadoDivisiones = await conexion
                .QueryAsync<Division>(sentenciaSQL, new DynamicParameters());

            return resultadoDivisiones;
        }

        public async Task<IEnumerable<Clase>> GetAllClassesAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT division_nombre division, clase_nombre nombre, clase_id id " +
                "FROM v_info_botanica " +
                "ORDER BY division_nombre, clase_nombre";

            var resultadoClases = await conexion
                .QueryAsync<Clase>(sentenciaSQL, new DynamicParameters());

            return resultadoClases;
        }

        public async Task<IEnumerable<Orden>> GetAllOrdersAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT clase_nombre clase, orden_nombre nombre, orden_id id " +
                "FROM v_info_botanica " +
                "ORDER BY clase_nombre, orden_nombre";

            var resultadoOrdenes = await conexion
                .QueryAsync<Orden>(sentenciaSQL, new DynamicParameters());

            return resultadoOrdenes;
        }

        public async Task<IEnumerable<Familia>> GetAllFamiliesAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT orden_nombre orden, familia_nombre nombre, familia_id id " +
                "FROM v_info_botanica " +
                "ORDER BY orden_nombre, familia_nombre";

            var resultadoFamilias = await conexion
                .QueryAsync<Familia>(sentenciaSQL, new DynamicParameters());

            return resultadoFamilias;
        }
    }
}
