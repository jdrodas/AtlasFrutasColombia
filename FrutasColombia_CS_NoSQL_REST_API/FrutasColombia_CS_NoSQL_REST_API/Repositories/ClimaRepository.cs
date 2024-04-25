
using FrutasColombia_CS_NoSQL_REST_API.DbContexts;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;
using MongoDB.Driver;

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

        public async Task<Clima> GetByNameAsync(string clima_nombre)
        {
            Clima unClima = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionClimas = conexion
                .GetCollection<Clima>(contextoDB.ConfiguracionColecciones.ColeccionClimas);

            var resultado = await coleccionClimas
                .Find(clima => clima.Nombre!.ToLower().Equals(clima_nombre.ToLower()))
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

        public async Task<long> GetTotalByAltitudeRangeAsync(int altitud_minima, int altitud_maxima)
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionClimas = conexion
                .GetCollection<Clima>(contextoDB.ConfiguracionColecciones.ColeccionClimas);

            var builder = Builders<Clima>.Filter;
            var filtro = builder.And(
                builder.Eq(clima => clima.Altitud_Minima, altitud_minima),
                builder.Eq(clima => clima.Altitud_Maxima, altitud_maxima));

            var resultado = await coleccionClimas
                .CountDocumentsAsync(filtro);

            return resultado;
        }

        public async Task<bool> CreateAsync(Clima unClima)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionClimas = conexion.GetCollection<Clima>(contextoDB.ConfiguracionColecciones.ColeccionClimas);

            await coleccionClimas
                .InsertOneAsync(unClima);

            var resultado = await coleccionClimas
                .Find(clima => clima.Nombre == unClima.Nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Clima unClima)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionClimas = conexion.GetCollection<Clima>(contextoDB.ConfiguracionColecciones.ColeccionClimas);

            var resultado = await coleccionClimas
                .ReplaceOneAsync(clima => clima.Id == unClima.Id, unClima);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
        public async Task<bool> RemoveAsync(string clima_id)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionClimas = conexion.GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionClimas);

            var resultado = await coleccionClimas
                .DeleteOneAsync(clima => clima.Id == clima_id);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
