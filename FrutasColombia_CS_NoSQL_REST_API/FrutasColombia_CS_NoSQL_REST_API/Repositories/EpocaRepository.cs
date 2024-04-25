using FrutasColombia_CS_NoSQL_REST_API.DbContexts;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;
using MongoDB.Driver;

namespace FrutasColombia_CS_NoSQL_REST_API.Repositories
{
    public class EpocaRepository(MongoDbContext unContexto) : IEpocaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Epoca>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionEpocas = conexion
                .GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionEpocas);

            var lasEpocas = await coleccionEpocas
                .Find(_ => true)
                .SortBy(epoca => epoca.Nombre)
                .ToListAsync();

            return lasEpocas;
        }

        public async Task<Epoca> GetByIdAsync(string epoca_id)
        {
            Epoca unaEpoca = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEpocas = conexion
                .GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionEpocas);

            var resultado = await coleccionEpocas
                .Find(epoca => epoca.Id == epoca_id)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaEpoca = resultado;

            return unaEpoca;
        }

        public async Task<Epoca> GetByNameAsync(string epoca_nombre)
        {
            Epoca unaEpoca = new();

            var conexion = contextoDB.CreateConnection();
            var coleccionEpocas = conexion
                .GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionEpocas);

            var resultado = await coleccionEpocas
                .Find(epoca => epoca.Nombre!.ToLower().Equals(epoca_nombre.ToLower()))
                .FirstOrDefaultAsync();

            if (resultado is not null)
                unaEpoca = resultado;

            return unaEpoca;
        }

        public async Task<long> GetTotalByMonthRangeAsync(int mes_inicio, int mes_final)
        {
            var conexion = contextoDB.CreateConnection();
            var coleccionEpocas = conexion
                .GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionEpocas);

            var builder = Builders<Epoca>.Filter;
            var filtro = builder.And(
                builder.Eq(epoca => epoca.Mes_Inicio, mes_inicio),
                builder.Eq(epoca => epoca.Mes_Final, mes_final));

            var resultado = await coleccionEpocas
                .CountDocumentsAsync(filtro);

            return resultado;
        }

        //public async Task<int> GetTotalProductionById(Guid epoca_id)
        //{
        //    var conexion = contextoDB.CreateConnection();

        //    //Validamos si hay registros existentes
        //    DynamicParameters parametrosSentencia = new();
        //    parametrosSentencia.Add("@epoca_id", epoca_id,
        //                            DbType.Guid, ParameterDirection.Input);

        //    //Aqui colocamos la informacion nutricional
        //    string sentenciaSQL = "SELECT count(*) totalRegistros " +
        //        "FROM core.produccion_frutas " +
        //        "WHERE epoca_id = @epoca_id";

        //    var totalRegistros = await conexion
        //        .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

        //    return totalRegistros.FirstOrDefault();
        //}

        public async Task<bool> CreateAsync(Epoca unaEpoca)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEpocas = conexion.GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionEpocas);

            await coleccionEpocas
                .InsertOneAsync(unaEpoca);

            var resultado = await coleccionEpocas
                .Find(epoca => epoca.Nombre == unaEpoca.Nombre)
                .FirstOrDefaultAsync();

            if (resultado is not null)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> UpdateAsync(Epoca unaEpoca)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEpocas = conexion.GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionEpocas);

            var resultado = await coleccionEpocas
                .ReplaceOneAsync(epoca => epoca.Id == unaEpoca.Id, unaEpoca);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }

        public async Task<bool> RemoveAsync(string epoca_id)
        {
            bool resultadoAccion = false;

            var conexion = contextoDB.CreateConnection();
            var coleccionEpocas = conexion.GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionEpocas);

            var resultado = await coleccionEpocas
                .DeleteOneAsync(epoca => epoca.Id == epoca_id);

            if (resultado.IsAcknowledged)
                resultadoAccion = true;

            return resultadoAccion;
        }
    }
}
