using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using System.Data;

namespace FrutasColombia_CS_REST_API.Repositories
{
    public class MesRepository(PgsqlDbContext unContexto) : IMesRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Mes>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT mes_id id, mes_nombre nombre " +
                                  "FROM core.v_info_meses " +
                                  "ORDER BY id";

            var resultadoClimas = await conexion
                .QueryAsync<Mes>(sentenciaSQL, new DynamicParameters());

            return resultadoClimas.ToList();
        }

        public async Task<Mes> GetByIdAsync(int mes_id)
        {
            Mes unMes = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@mes_id", mes_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT mes_id id, mes_nombre nombre " +
                                  "FROM core.v_info_meses " +
                                  "WHERE mes_id = @mes_id";

            var resultado = await conexion
                .QueryAsync<Mes>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unMes = resultado.First();

            return unMes;
        }
    }
}
