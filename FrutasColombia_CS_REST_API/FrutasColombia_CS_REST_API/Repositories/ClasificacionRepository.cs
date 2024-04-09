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

        public async Task<IEnumerable<Genero>> GetAllGenusAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT familia_nombre familia, genero_nombre nombre, " +
                "genero_id id, count(especie_id) total_especies " +
                "FROM v_info_botanica " +
                "GROUP BY familia_nombre, genero_nombre, genero_id " +
                "ORDER BY familia_nombre, genero_nombre";

            var resultadoGeneros = await conexion
                .QueryAsync<Genero>(sentenciaSQL, new DynamicParameters());

            return resultadoGeneros;
        }

        public async Task<Genero> GetGenusByIdAsync(int genero_id)
        {
            Genero unGenero = new();

            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT v.genero_id id, v.genero_nombre nombre, " +
                "v.familia_nombre familia, count(v.especie_id) total_especies " +
                "FROM core.v_info_botanica v " +
                "WHERE v.genero_id = @genero_id " +
                "GROUP BY v.genero_id, v.genero_nombre, v.familia_nombre " +
                "ORDER BY v.genero_nombre; ";

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@genero_id", genero_id,
                                    DbType.Int32, ParameterDirection.Input);

            var resultado = await conexion
                .QueryAsync<Genero>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unGenero = resultado.First();

            return unGenero;
        }

        public async Task<GeneroDetallado> GetGenusDetailsByIdAsync(int genero_id)
        {
            Genero unGenero = await GetGenusByIdAsync(genero_id);

            GeneroDetallado unGeneroDetallado = new()
            {
                Id = unGenero.Id,
                Nombre = unGenero.Nombre,
                Familia = unGenero.Familia,
                Total_Especies = unGenero.Total_Especies,
                Especies = await GetAssociatedSpeciesToGenusById(genero_id)
            };

            return unGeneroDetallado;
        }

        public async Task<List<Especie>> GetAssociatedSpeciesToGenusById(int genero_id)
        {
            List<Especie> especiesDelGenero = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@genero_id", genero_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT especie_id id, especie_nombre nombre " +
                "FROM core.v_info_botanica v " +
                "WHERE v.genero_id = @genero_id " +
                "ORDER BY v.especie_nombre";

            var resultadoEspecies = await conexion
                .QueryAsync<Especie>(sentenciaSQL, parametrosSentencia);

            if (resultadoEspecies.Any())
                especiesDelGenero = resultadoEspecies.ToList();

            return especiesDelGenero;
        }
    }
}
