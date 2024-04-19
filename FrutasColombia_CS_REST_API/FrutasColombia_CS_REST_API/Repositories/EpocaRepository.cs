using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
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
                .QueryAsync<Epoca>(sentenciaSQL,parametrosSentencia);

            if (resultado.Any())
                unaEpoca = resultado.First();

            return unaEpoca;
        }
    }
}
