using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using Npgsql;
using System.Data;

namespace FrutasColombia_CS_REST_API.Repositories
{
    public class EpocaRepository(PgsqlDbContext unContexto) : IEpocaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Epoca>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT id, nombre, mes_inicio, mes_final " +
                                  "FROM core.epocas " +
                                  "ORDER BY id";

            var resultadoEpocas = await conexion
                .QueryAsync<Epoca>(sentenciaSQL, new DynamicParameters());

            return resultadoEpocas;
        }

        public async Task<Epoca> GetByIdAsync(Guid epoca_id)
        {
            Epoca unaEpoca = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@epoca_id", epoca_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, mes_inicio, mes_final " +
                                  "FROM core.epocas " +
                                  "WHERE id = @epoca_id";

            var resultado = await conexion
                .QueryAsync<Epoca>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaEpoca = resultado.First();

            return unaEpoca;
        }

        public async Task<Epoca> GetByNameAsync(string epoca_nombre)
        {
            Epoca unaEpoca = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@epoca_nombre", epoca_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, mes_inicio, mes_final " +
                                  "FROM core.epocas " +
                                  "WHERE nombre = @epoca_nombre";

            var resultado = await conexion
                .QueryAsync<Epoca>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unaEpoca = resultado.First();

            return unaEpoca;
        }

        public async Task<int> GetTotalByMonthRangeAsync(int mes_inicio, int mes_final)
        {
            var conexion = contextoDB.CreateConnection();

            //Validamos si hay registros existentes
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@mes_inicio", mes_inicio,
                                    DbType.Int32, ParameterDirection.Input);

            parametrosSentencia.Add("@mes_final", mes_final,
                                    DbType.Int32, ParameterDirection.Input);

            //Aqui colocamos la informacion nutricional
            string sentenciaSQL = "SELECT count(*) totalRegistros " +
                "FROM core.epocas " +
                "WHERE mes_inicio = @mes_inicio AND mes_final = @mes_final";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalRegistros.FirstOrDefault();
        }

        public async Task<int> GetTotalProductionById(Guid epoca_id)
        {
            var conexion = contextoDB.CreateConnection();

            //Validamos si hay registros existentes
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@epoca_id", epoca_id,
                                    DbType.Guid, ParameterDirection.Input);

            //Aqui colocamos la informacion nutricional
            string sentenciaSQL = "SELECT count(*) totalRegistros " +
                "FROM core.produccion_frutas " +
                "WHERE epoca_id = @epoca_id";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalRegistros.FirstOrDefault();
        }

        public async Task<bool> CreateAsync(Epoca unaEpoca)
        {
            bool resultadoAccion = false;

            //Validamos si la fruta existe por ese Nombre
            var epocaExistente = await GetByNameAsync(unaEpoca.Nombre!);

            if (epocaExistente.Id != Guid.Empty)
                throw new DbOperationException($"No se puede insertar. Ya existe la época {unaEpoca.Nombre!}.");

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_epoca";
                var parametros = new
                {
                    p_nombre = unaEpoca.Nombre,
                    p_mes_inicio = unaEpoca.Mes_Inicio,
                    p_mes_final = unaEpoca.Mes_Final
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

        public async Task<bool> UpdateAsync(Epoca unaEpoca)
        {
            bool resultadoAccion = false;

            //Validamos si la fruta existe por ese ID
            var epocaExistente = await GetByIdAsync(unaEpoca.Id);

            if (epocaExistente.Id == Guid.Empty)
                throw new DbOperationException($"No se puede actualizar. No existe la época {unaEpoca.Nombre!}.");

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_epoca";
                var parametros = new
                {
                    p_id = unaEpoca.Id,
                    p_nombre = unaEpoca.Nombre,
                    p_mes_inicio = unaEpoca.Mes_Inicio,
                    p_mes_final = unaEpoca.Mes_Final
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

        public async Task<bool> RemoveAsync(Guid epoca_id)
        {
            bool resultadoAccion = false;

            //Validamos si la epoca existe por ese ID
            var epocaExistente = await GetByIdAsync(epoca_id);

            if (epocaExistente.Id == Guid.Empty)
                throw new DbOperationException($"No se puede eliminar. No existe la época con el Id {epoca_id}.");

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_epoca";
                var parametros = new
                {
                    p_id = epoca_id
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
