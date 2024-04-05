using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

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
    }
}
