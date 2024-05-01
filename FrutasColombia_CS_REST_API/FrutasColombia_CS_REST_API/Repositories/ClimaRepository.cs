using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using Npgsql;
using System.Data;

namespace FrutasColombia_CS_REST_API.Repositories
{
    public class ClimaRepository(PgsqlDbContext unContexto) : IClimaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Clima>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT id, nombre, altitud_minima, altitud_maxima " +
                                  "FROM core.climas " +
                                  "ORDER BY altitud_minima";

            var resultadoClimas = await conexion
                .QueryAsync<Clima>(sentenciaSQL, new DynamicParameters());

            return resultadoClimas.ToList();
        }

        public async Task<Clima> GetByIdAsync(Guid clima_id)
        {
            Clima unClima = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@clima_id", clima_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, altitud_minima, altitud_maxima " +
                                  "FROM core.climas " +
                                  "WHERE id = @clima_id";

            var resultado = await conexion
                .QueryAsync<Clima>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unClima = resultado.First();

            return unClima;
        }

        public async Task<Clima> GetByNameAsync(string clima_nombre)
        {
            Clima unClima = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@clima_nombre", clima_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, altitud_minima, altitud_maxima " +
                                  "FROM core.climas " +
                                  "WHERE LOWER(nombre) = LOWER(@clima_nombre)";

            var resultado = await conexion
                .QueryAsync<Clima>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unClima = resultado.First();

            return unClima;
        }

        public async Task<Clima> GetByAltitudeRangeAsync(int altitud_minima, int altitud_maxima)
        {
            Clima unClima = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@altitud_minima", altitud_minima,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@altitud_maxima", altitud_maxima,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, altitud_minima, altitud_maxima " +
                                  "FROM core.climas " +
                                  "WHERE altitud_minima = @altitud_minima " +
                                  "AND altitud_maxima = @altitud_maxima";

            var resultado = await conexion
                .QueryAsync<Clima>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unClima = resultado.First();

            return unClima;
        }

        public async Task<int> GetTotalByAltitudeRangeAsync(int altitud_minima, int altitud_maxima)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@altitud_minima", altitud_minima,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@altitud_maxima", altitud_maxima,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT count(c.id) totalRegistros " +
                "FROM core.climas c " +
                "WHERE @altitud_minima = c.altitud_minima " +
                "AND @altitud_maxima = c.altitud_maxima";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalRegistros.FirstOrDefault();
        }

        public async Task<int> GetTotalProductionById(Guid clima_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@clima_id", clima_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL = "SELECT count(*) totalRegistros " +
                "FROM core.produccion_frutas " +
                "WHERE clima_id = @clima_id";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalRegistros.FirstOrDefault();
        }

        public async Task<string> ValidateAltitudeRangeAsync(Clima unClima)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@altitud_minima", unClima.Altitud_Minima,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@altitud_maxima", unClima.Altitud_Maxima,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@clima_id", unClima.Id,
                        DbType.Guid, ParameterDirection.Input);

            //Validar que la altura mínima no esté en un rango existente
            string sentenciaSQL = "SELECT count(c.id) totalRegistros " +
                "FROM core.climas c " +
                "WHERE @altitud_minima BETWEEN c.altitud_minima AND c.altitud_maxima " +
                "AND @altitud_minima != c.altitud_maxima " +
                "AND c.id != @clima_id";

            var resultado = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            if (resultado.FirstOrDefault() > 0)
                return $"La altura mínima ya está incluida en otro rango";

            //Validar que la altura máxima no esté un rango existente
            sentenciaSQL = "SELECT count(c.id) totalRegistros " +
                "FROM core.climas c " +
                "WHERE @altitud_maxima BETWEEN c.altitud_minima AND c.altitud_maxima " +
                "AND c.id != @clima_id";

            resultado = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            if (resultado.FirstOrDefault() > 0)
                return $"La altura máxima ya está incluida en otro rango";

            return string.Empty;
        }

        public async Task<bool> CreateAsync(Clima unClima)
        {
            bool resultadoAccion = false;

            //Validamos si la fruta existe por ese Nombre
            var climaExistente = await GetByNameAsync(unClima.Nombre!);

            if (climaExistente.Id != Guid.Empty)
                throw new DbOperationException($"No se puede insertar. Ya existe el clima {unClima.Nombre!}.");

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_clima";
                var parametros = new
                {
                    p_nombre = unClima.Nombre,
                    p_altitud_minima = unClima.Altitud_Minima,
                    p_altitud_maxima = unClima.Altitud_Maxima
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

        public async Task<bool> UpdateAsync(Clima unClima)
        {
            bool resultadoAccion = false;

            var climaExistente = await GetByIdAsync(unClima.Id);

            if (climaExistente.Id == Guid.Empty)
                throw new DbOperationException($"No se puede actualizar. No existe un clima {unClima.Nombre!}.");

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_clima";
                var parametros = new
                {
                    p_id = unClima.Id,
                    p_nombre = unClima.Nombre,
                    p_altitud_minima = unClima.Altitud_Minima,
                    p_altitud_maxima = unClima.Altitud_Maxima
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

        public async Task<bool> RemoveAsync(Guid clima_id)
        {
            bool resultadoAccion = false;

            var climaExistente = await GetByIdAsync(clima_id);

            if (climaExistente.Id == Guid.Empty)
                throw new DbOperationException($"No se puede eliminar. No existe el clima con el Id {clima_id}.");

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_clima";
                var parametros = new
                {
                    p_id = clima_id
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
    }
}
