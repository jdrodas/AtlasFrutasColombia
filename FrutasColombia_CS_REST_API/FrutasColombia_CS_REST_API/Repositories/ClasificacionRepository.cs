using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using Npgsql;
using System.Data;

namespace FrutasColombia_CS_REST_API.Repositories
{
    public class ClasificacionRepository(PgsqlDbContext unContexto) : IClasificacionRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Clasificacion>> GetAllAsync()
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

            return resultadoClasificacion.ToList();
        }

        public async Task<List<Division>> GetAllDivisionsAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT reino_nombre reino, division_nombre nombre, division_id id " +
                "FROM v_info_botanica " +
                "ORDER BY reino_nombre, division_nombre";

            var resultadoDivisiones = await conexion
                .QueryAsync<Division>(sentenciaSQL, new DynamicParameters());

            return resultadoDivisiones.ToList();
        }

        public async Task<List<Clase>> GetAllClassesAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT division_nombre division, clase_nombre nombre, clase_id id " +
                "FROM v_info_botanica " +
                "ORDER BY division_nombre, clase_nombre";

            var resultadoClases = await conexion
                .QueryAsync<Clase>(sentenciaSQL, new DynamicParameters());

            return resultadoClases.ToList();
        }

        public async Task<List<Orden>> GetAllOrdersAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT clase_nombre clase, orden_nombre nombre, orden_id id " +
                "FROM v_info_botanica " +
                "ORDER BY clase_nombre, orden_nombre";

            var resultadoOrdenes = await conexion
                .QueryAsync<Orden>(sentenciaSQL, new DynamicParameters());

            return resultadoOrdenes.ToList();
        }

        public async Task<List<Familia>> GetAllFamiliesAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT orden_nombre orden, familia_nombre nombre, familia_id id " +
                "FROM v_info_botanica " +
                "ORDER BY orden_nombre, familia_nombre";

            var resultadoFamilias = await conexion
                .QueryAsync<Familia>(sentenciaSQL, new DynamicParameters());

            return resultadoFamilias.ToList();
        }

        public async Task<List<Genero>> GetAllGenusAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT DISTINCT familia_nombre familia, genero_nombre nombre, " +
                "genero_id id, count(especie_id) total_especies " +
                "FROM v_info_botanica " +
                "GROUP BY familia_nombre, genero_nombre, genero_id " +
                "ORDER BY familia_nombre, genero_nombre";

            var resultadoGeneros = await conexion
                .QueryAsync<Genero>(sentenciaSQL, new DynamicParameters());

            return resultadoGeneros.ToList();
        }

        public async Task<Genero> GetGenusByIdAsync(Guid genero_id)
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
                                    DbType.Guid, ParameterDirection.Input);

            var resultado = await conexion
                .QueryAsync<Genero>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unGenero = resultado.First();

            return unGenero;
        }

        public async Task<GeneroDetallado> GetGenusDetailsByIdAsync(Guid genero_id)
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

        public async Task<List<Especie>> GetAssociatedSpeciesToGenusById(Guid genero_id)
        {
            List<Especie> especiesDelGenero = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@genero_id", genero_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT especie_id id, especie_nombre nombre " +
                "FROM core.v_info_botanica v " +
                "WHERE v.genero_id = @genero_id " +
                "ORDER BY v.especie_nombre";

            var resultado = await conexion
                .QueryAsync<Especie>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                especiesDelGenero = resultado.ToList();

            return especiesDelGenero;
        }

        public async Task<Guid> GetTaxonomicElementIdAsync(string tipo_elemento, string nombre_elemento)
        {
            Guid elemento_guid = Guid.Empty;

            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT id ";

            switch (tipo_elemento)
            {
                case "reino":
                    sentenciaSQL += "FROM core.reinos ";
                    break;

                case "division":
                    sentenciaSQL += "FROM core.divisiones ";
                    break;

                case "orden":
                    sentenciaSQL += "FROM core.ordenes ";
                    break;

                case "clase":
                    sentenciaSQL += "FROM core.clases ";
                    break;

                case "familia":
                    sentenciaSQL += "FROM core.familias ";
                    break;

                case "genero":
                    sentenciaSQL += "FROM core.generos ";
                    break;

                case "especie":
                    sentenciaSQL += "FROM core.especies ";
                    break;
            }

            sentenciaSQL += "WHERE nombre = @nombre_elemento";

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@nombre_elemento", nombre_elemento,
                                    DbType.String, ParameterDirection.Input);

            var resultado = await conexion
                .QueryAsync<Guid>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                elemento_guid = resultado.First();

            return elemento_guid;
        }

        public async Task<bool> CreateAsync(Taxonomia unaClasificacion)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalTaxonomicRecordsAsync(unaClasificacion);

            if (totalRegistros != 0)
                throw new DbOperationException("No se puede insertar. Esta información ya está registrada.");

            try
            {
                var conexion = contextoDB.CreateConnection();
                string procedimiento = "core.p_inserta_clasificacion";
                var parametros = new
                {
                    p_reino = unaClasificacion.Reino,
                    p_division = unaClasificacion.Division,
                    p_clase = unaClasificacion.Clase,
                    p_orden = unaClasificacion.Orden,
                    p_familia = unaClasificacion.Familia,
                    p_genero = unaClasificacion.Genero,
                    p_especie = unaClasificacion.Especie
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(Taxonomia unaClasificacion)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalTaxonomicRecordsAsync(unaClasificacion);

            if (totalRegistros == 0)
                throw new DbOperationException("No se puede eliminar. Esta información no está registrada.");

            try
            {
                var conexion = contextoDB.CreateConnection();
                string procedimiento = "core.p_elimina_clasificacion";
                var parametros = new
                {
                    p_reino = unaClasificacion.Reino,
                    p_division = unaClasificacion.Division,
                    p_clase = unaClasificacion.Clase,
                    p_orden = unaClasificacion.Orden,
                    p_familia = unaClasificacion.Familia,
                    p_genero = unaClasificacion.Genero,
                    p_especie = unaClasificacion.Especie
                };

                var cantidad_filas = await conexion.ExecuteAsync(
                    procedimiento,
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (cantidad_filas != 0)
                    resultadoAccion = true;
            }
            catch (NpgsqlException error)
            {
                throw new DbOperationException(error.Message);
            }

            return resultadoAccion;
        }

        private async Task<int> GetTotalTaxonomicRecordsAsync(Taxonomia unaTaxonomia)
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "select count(*) totalRegistros " +
                "FROM v_info_botanica " +
                "WHERE reino_nombre = @Reino " +
                "AND division_nombre = @Division " +
                "AND clase_nombre = @Clase " +
                "AND orden_nombre = @Orden " +
                "AND familia_nombre = @Familia " +
                "AND genero_nombre = @Genero " +
                "AND especie_nombre = @Especie";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, unaTaxonomia);

            return totalRegistros.FirstOrDefault();
        }
    }
}
