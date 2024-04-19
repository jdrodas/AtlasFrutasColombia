
using FrutasColombia_CS_NoSQL_REST_API.DbContexts;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;
using System.Data;

namespace FrutasColombia_CS_NoSQL_REST_API.Repositories
{
    public class MunicipioRepository(MongoDbContext unContexto) : IMunicipioRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Municipio>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT m.id, m.nombre, d.nombre departamento " +
                "FROM core.municipios m " +
                "JOIN core.departamentos d ON m.departamento_id = d.id " +
                "ORDER BY d.nombre, m.nombre";

            var resultadoMunicipios = await conexion
                .QueryAsync<Municipio>(sentenciaSQL, new DynamicParameters());

            return resultadoMunicipios;
        }

        public async Task<Municipio> GetByIdAsync(string? municipio_id)
        {
            Municipio unMunicipio = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@municipio_id", municipio_id,
                                    DbType.string?, ParameterDirection.Input);

            string sentenciaSQL = "SELECT m.id, m.nombre, d.nombre departamento " +
                "FROM core.municipios m " +
                "JOIN core.departamentos d ON m.departamento_id = d.id " +
                "WHERE m.id = @municipio_id ";

            var resultado = await conexion
                .QueryAsync<Municipio>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                unMunicipio = resultado.First();

            return unMunicipio;
        }
    }
}
