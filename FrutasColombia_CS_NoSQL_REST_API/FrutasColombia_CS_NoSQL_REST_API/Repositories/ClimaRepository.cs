
using FrutasColombia_CS_NoSQL_REST_API.DbContexts;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;
using MongoDB.Driver;
using System.Data;

namespace FrutasColombia_CS_NoSQL_REST_API.Repositories
{
    public class ClimaRepository(MongoDbContext unContexto) : IClimaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Clima>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionClimas = conexion
                .GetCollection<Clima>(contextoDB.ConfiguracionColecciones.ColeccionClimas);

            var losClimas = await coleccionClimas
                .Find(_ => true)
                .SortBy(clima => clima.Nombre)
                .ToListAsync();

            return losClimas;
        }

        public async Task<Clima> GetByIdAsync(string clima_id)
        {
            Clima unClima = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionClimas = conexion
                .GetCollection<Clima>(contextoDB.ConfiguracionColecciones.ColeccionClimas);

            var resultado = await coleccionClimas
                .Find(clima => clima.Id == clima_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unClima = resultado;

            return unClima;
        }

        //public async Task<Clima> GetByIdAsync(string? clima_id)
        //{
        //    Clima unClima = new();

        //    var conexion = contextoDB.CreateConnection();

        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@clima_id", clima_id,
        //                            DbType.string?, ParameterDirection.Input);

        //    string sentenciaSQL = "SELECT id, nombre, altitud_minima, altitud_maxima " +
        //                          "FROM core.climas " +
        //                          "WHERE id = @clima_id";

        //    var resultado = await conexion.QueryAsync<Clima>(sentenciaSQL,
        //        parametrosSentencia);

        //    if (resultado.Any())
        //        unClima = resultado.First();

        //    return unClima;
        //}
    }
}
