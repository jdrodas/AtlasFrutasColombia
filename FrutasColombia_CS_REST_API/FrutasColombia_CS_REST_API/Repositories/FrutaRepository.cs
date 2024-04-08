using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Helpers;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using Npgsql;
using System.Data;

namespace FrutasColombia_CS_REST_API.Repositories
{
    public class FrutaRepository(PgsqlDbContext unContexto) : IFrutaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Fruta>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                  "FROM core.frutas " +
                                  "ORDER BY nombre";

            var resultadoFrutas = await conexion
                .QueryAsync<Fruta>(sentenciaSQL, new DynamicParameters());

            return resultadoFrutas;
        }

        public async Task<Fruta> GetByIdAsync(int fruta_id)
        {
            Fruta unaFruta = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                  "FROM core.frutas " +
                                  "WHERE id = @fruta_id " +
                                  "ORDER BY nombre";

            var resultado = await conexion.QueryAsync<Fruta>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unaFruta = resultado.First();

            return unaFruta;
        }

        public async Task<Fruta> GetByNameAsync(string fruta_nombre)
        {
            Fruta unaFruta = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_nombre", fruta_nombre,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                  "FROM core.frutas " +
                                  "WHERE nombre = @fruta_nombre " +
                                  "ORDER BY nombre";

            var resultado = await conexion.QueryAsync<Fruta>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unaFruta = resultado.First();

            return unaFruta;
        }

        public async Task<IEnumerable<FrutaProducida>> GetByLocationAsync(string? departamento_id, string? municipio_id)
        {
            List<FrutaProducida> frutasProducidas = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            string sentenciaSQL = "SELECT DISTINCT fruta_id id, fruta_nombre nombre, " +
                "fruta_wikipedia url_wikipedia, fruta_imagen url_imagen " +
                "FROM v_info_produccion_frutas v ";

            if (!string.IsNullOrEmpty(departamento_id))
            {
                parametrosSentencia.Add("@departamento_id", departamento_id,
                DbType.String, ParameterDirection.Input);

                sentenciaSQL += "WHERE v.departamento_id = @departamento_id";
            }

            if (!string.IsNullOrEmpty(municipio_id))
            {
                parametrosSentencia.Add("@municipio_id", municipio_id,
                DbType.String, ParameterDirection.Input);

                sentenciaSQL += "WHERE v.municipio_id = @municipio_id";
            }

            var resultadoFrutas = await conexion
                .QueryAsync<FrutaProducida>(sentenciaSQL, parametrosSentencia);

            if (resultadoFrutas.Any())
            {
                frutasProducidas = resultadoFrutas.ToList();

                foreach (FrutaProducida unaFruta in frutasProducidas.ToList())
                {
                    if (!string.IsNullOrEmpty(departamento_id))
                        unaFruta.Produccion = await GetFruitProductionDetails(unaFruta.Id, departamento_id, null);

                    if (!string.IsNullOrEmpty(municipio_id))
                        unaFruta.Produccion = await GetFruitProductionDetails(unaFruta.Id, null, municipio_id);
                }
            }

            return frutasProducidas;
        }

        public async Task<IEnumerable<FrutaProducida>> GetByClimateAsync(int clima_id)
        {
            List<FrutaProducida> frutasProducidas = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@clima_id", clima_id,
                DbType.Int32, ParameterDirection.Input);


            string sentenciaSQL = "SELECT DISTINCT fruta_id id, fruta_nombre nombre, " +
                "fruta_wikipedia url_wikipedia, fruta_imagen url_imagen " +
                "FROM v_info_produccion_frutas v " +
                "WHERE clima_id = @clima_id";

            var resultadoFrutas = await conexion
                .QueryAsync<FrutaProducida>(sentenciaSQL, parametrosSentencia);

            if (resultadoFrutas.Any())
            {
                frutasProducidas = resultadoFrutas.ToList();

                foreach (FrutaProducida unaFruta in frutasProducidas.ToList())
                    unaFruta.Produccion = await GetFruitProductionDetails(unaFruta.Id, clima_id);
            }

            return frutasProducidas;
        }

        public async Task<FrutaDetallada> GetFruitDetailsByIdAsync(int fruta_id)
        {
            Fruta unaFruta = await GetByIdAsync(fruta_id);

            FrutaDetallada unaFrutaDetallada = new()
            {
                Id = unaFruta.Id,
                Nombre = unaFruta.Nombre,
                Url_Imagen = unaFruta.Url_Imagen,
                Url_Wikipedia = unaFruta.Url_Wikipedia,
                Produccion = await GetFruitProductionDetails(fruta_id, null, null),
                Nutricion = await GetFruitNutritionDetails(fruta_id),
                Taxonomia = await GetFruitTaxonomicDetails(fruta_id)
            };

            return unaFrutaDetallada;
        }

        public async Task<List<Produccion>> GetFruitProductionDetails(int fruta_id, string? departamento_id, string? municipio_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id ";

            if (!string.IsNullOrEmpty(departamento_id))
            {
                parametrosSentencia.Add("@departamento_id", departamento_id,
                DbType.String, ParameterDirection.Input);

                sentenciaSQL += "AND v.departamento_id = @departamento_id";
            }

            if (!string.IsNullOrEmpty(municipio_id))
            {
                parametrosSentencia.Add("@municipio_id", municipio_id,
                DbType.String, ParameterDirection.Input);

                sentenciaSQL += "AND v.municipio_id = @municipio_id";
            }

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultadoProduccion.Any())
                infoProduccion = resultadoProduccion.ToList();

            return infoProduccion;
        }

        public async Task<List<Produccion>> GetFruitProductionDetails(int fruta_id, int clima_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Int32, ParameterDirection.Input);

            parametrosSentencia.Add("@clima_id", clima_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id " +
                "AND v.clima_id = @clima_id";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultadoProduccion.Any())
                infoProduccion = resultadoProduccion.ToList();

            return infoProduccion;
        }

        public async Task<Nutricion> GetFruitNutritionDetails(int fruta_id)
        {
            Nutricion infoNutricion = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Int32, ParameterDirection.Input);

            //Aqui colocamos la informacion nutricional
            string sentenciaSQL = "SELECT DISTINCT azucares, carbohidratos, grasas, proteinas " +
                "FROM core.v_info_nutricion_frutas v " +
                "WHERE v.fruta_id = @fruta_id";

            var resultadoNutricion = await conexion
                .QueryAsync<Nutricion>(sentenciaSQL, parametrosSentencia);

            if (resultadoNutricion.Any())
                infoNutricion = resultadoNutricion.First();

            return infoNutricion;
        }

        public async Task<Taxonomia> GetFruitTaxonomicDetails(int fruta_id)
        {
            Taxonomia infoTaxonomia = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Int32, ParameterDirection.Input);

            //Aqui colocamos la informacion de clasificación taxonómica
            string sentenciaSQL = "SELECT DISTINCT reino_nombre reino, division_nombre division, clase_nombre clase, " +
                "orden_nombre orden, familia_nombre familia, " +
                "genero_nombre genero, especie_nombre especie " +
                "FROM core.v_info_frutas v " +
                "WHERE v.fruta_id = @fruta_id";

            var resultadoTaxonomico = await conexion
                .QueryAsync<Taxonomia>(sentenciaSQL, parametrosSentencia);

            if (resultadoTaxonomico.Any())
                infoTaxonomia = resultadoTaxonomico.First();

            return infoTaxonomia;
        }

        public async Task<bool> CreateAsync(Fruta unaFruta)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_inserta_fruta";
                var parametros = new
                {
                    p_nombre = unaFruta.Nombre,
                    p_url_wikipedia = unaFruta.Url_Wikipedia,
                    p_url_imagen = unaFruta.Url_Imagen
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

        public async Task<bool> UpdateAsync(Fruta unaFruta)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_fruta";
                var parametros = new
                {
                    p_id = unaFruta.Id,
                    p_nombre = unaFruta.Nombre,
                    p_url_wikipedia = unaFruta.Url_Wikipedia,
                    p_url_imagen = unaFruta.Url_Imagen
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

        public async Task<bool> RemoveAsync(int fruta_id)
        {
            bool resultadoAccion = false;

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_fruta";
                var parametros = new
                {
                    p_id = fruta_id
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
