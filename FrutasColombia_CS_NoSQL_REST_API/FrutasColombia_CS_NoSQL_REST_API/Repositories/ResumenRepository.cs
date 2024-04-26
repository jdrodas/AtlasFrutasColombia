
using FrutasColombia_CS_NoSQL_REST_API.DbContexts;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FrutasColombia_CS_NoSQL_REST_API.Repositories
{
    public class ResumenRepository(MongoDbContext unContexto) : IResumenRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<Resumen> GetAllAsync()
        {
            Resumen unResumen = new();

            var conexion = contextoDB.CreateConnection();

            // Total frutas
            var coleccionFrutas = conexion
                .GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionFrutas);

            unResumen.Frutas = await coleccionFrutas
                .CountDocumentsAsync(_ => true);
            
            // Total Epocas
            var coleccionEpocas = conexion
                .GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionEpocas);

            unResumen.Epocas = await coleccionEpocas
                .CountDocumentsAsync(_ => true);

            // Total Climas
            var coleccionClimas = conexion
                .GetCollection<Epoca>(contextoDB.ConfiguracionColecciones.ColeccionClimas);

            unResumen.Climas = await coleccionClimas
                .CountDocumentsAsync(_ => true);

            // Totales de Taxonomias -- Reinos
            var coleccionTaxonomias = conexion
                .GetCollection<Taxonomia>(contextoDB.ConfiguracionColecciones.ColeccionTaxonomias);

            var filtroPredeterminado = Builders<Taxonomia>.Filter.Empty;

            var resultado = await coleccionTaxonomias
                .DistinctAsync<string>("reino", filter: filtroPredeterminado);

            unResumen.Taxonomia_Reinos = resultado.ToList().Count;

            // Totales de Taxonomias -- Divisiones
            resultado = await coleccionTaxonomias
                .DistinctAsync<string>("division", filter: filtroPredeterminado);

            unResumen.Taxonomia_Divisiones = resultado.ToList().Count;

            // Totales de Taxonomias -- Divisiones
            resultado = await coleccionTaxonomias
                .DistinctAsync<string>("division", filter: filtroPredeterminado);

            unResumen.Taxonomia_Divisiones = resultado.ToList().Count;

            // Totales de Taxonomias -- clases
            resultado = await coleccionTaxonomias
                .DistinctAsync<string>("clase", filter: filtroPredeterminado);

            unResumen.Taxonomia_Clases = resultado.ToList().Count;

            // Totales de Taxonomias -- ordenes
            resultado = await coleccionTaxonomias
                .DistinctAsync<string>("orden", filter: filtroPredeterminado);

            unResumen.Taxonomia_Ordenes = resultado.ToList().Count;

            // Totales de Taxonomias -- familias
            resultado = await coleccionTaxonomias
                .DistinctAsync<string>("familia", filter: filtroPredeterminado);

            unResumen.Taxonomia_Familias = resultado.ToList().Count;

            // Totales de Taxonomias -- generos
            resultado = await coleccionTaxonomias
                .DistinctAsync<string>("genero", filter: filtroPredeterminado);

            unResumen.Taxonomia_Generos = resultado.ToList().Count;

            resultado = await coleccionTaxonomias
                .DistinctAsync<string>("especie", filter: filtroPredeterminado);

            unResumen.Taxonomia_Especies = resultado.ToList().Count;

            //sentenciaSQL = "SELECT COUNT(id) total FROM core.departamentos";
            //unResumen.Departamentos = await conexion
            //    .QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

            //sentenciaSQL = "SELECT COUNT(id) total FROM core.municipios";
            //unResumen.Municipios = await conexion
            //    .QueryFirstAsync<int>(sentenciaSQL, new DynamicParameters());

            return unResumen;
        }
    }
}