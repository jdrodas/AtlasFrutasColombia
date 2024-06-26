﻿
using FrutasColombia_CS_NoSQL_REST_API.DbContexts;
using FrutasColombia_CS_NoSQL_REST_API.Helpers;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;

using System.Data;


namespace FrutasColombia_CS_NoSQL_REST_API.Repositories
{
    public class FrutaRepository(MongoDbContext unContexto) : IFrutaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

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

        public async Task<Fruta> GetByIdAsync(string? fruta_id)
        {
            Fruta unaFruta = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.string?, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                  "FROM core.frutas " +
                                  "WHERE id = @fruta_id " +
                                  "ORDER BY nombre";

            var resultado = await conexion
                .QueryAsync<Fruta>(sentenciaSQL, parametrosSentencia);

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

            var resultado = await conexion
                .QueryAsync<Fruta>(sentenciaSQL, parametrosSentencia);

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

            if (departamento_id != string?.Empty)
            {
                parametrosSentencia.Add("@departamento_id", departamento_id,
                DbType.string?, ParameterDirection.Input);

                sentenciaSQL += "WHERE v.departamento_id = @departamento_id";
            }

            if (municipio_id != string?.Empty)
            {
                parametrosSentencia.Add("@municipio_id", municipio_id,
                DbType.string?, ParameterDirection.Input);

                sentenciaSQL += "WHERE v.municipio_id = @municipio_id";
            }

            var resultadoFrutas = await conexion
                .QueryAsync<FrutaProducida>(sentenciaSQL, parametrosSentencia);

            if (resultadoFrutas.Any())
            {
                frutasProducidas = resultadoFrutas.ToList();

                foreach (FrutaProducida unaFruta in frutasProducidas.ToList())
                {
                    if (departamento_id != string?.Empty)
                        unaFruta.Produccion = await GetProductionDetailsAsync(unaFruta.Id, departamento_id, string?.Empty);

                    if (municipio_id != string?.Empty)
                        unaFruta.Produccion = await GetProductionDetailsAsync(unaFruta.Id, string?.Empty, municipio_id);
                }
            }

            return frutasProducidas;
        }

        public async Task<IEnumerable<FrutaProducida>> GetByClimateAsync(string? clima_id)
        {
            List<FrutaProducida> frutasProducidas = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@clima_id", clima_id,
                DbType.string?, ParameterDirection.Input);


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
                    unaFruta.Produccion = await GetProductionDetailsAsync(unaFruta.Id, clima_id);
            }

            return frutasProducidas;
        }

        public async Task<IEnumerable<FrutaProducida>> GetByMonthAsync(int mes_id)
        {
            List<FrutaProducida> frutasProducidas = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@mes_id", mes_id,
                DbType.Int32, ParameterDirection.Input);


            string sentenciaSQL = "SELECT DISTINCT fruta_id id, fruta_nombre nombre, " +
                "fruta_wikipedia url_wikipedia, fruta_imagen url_imagen " +
                "FROM v_info_produccion_frutas v " +
                "WHERE @mes_id BETWEEN mes_inicio and mes_final";

            var resultadoFrutas = await conexion
                .QueryAsync<FrutaProducida>(sentenciaSQL, parametrosSentencia);

            if (resultadoFrutas.Any())
            {
                frutasProducidas = resultadoFrutas.ToList();

                foreach (FrutaProducida unaFruta in frutasProducidas.ToList())
                    unaFruta.Produccion = await GetProductionDetailsAsync(unaFruta.Id, mes_id);
            }

            return frutasProducidas;
        }

        public async Task<IEnumerable<FrutaClasificada>> GetByGenusIdAsync(string? genero_id)
        {
            List<FrutaClasificada> frutasClasificadas = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@genero_id", genero_id,
                DbType.string?, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT fruta_id id, fruta_nombre nombre, " +
                "url_wikipedia, url_imagen " +
                "FROM v_info_frutas v " +
                "WHERE genero_id = @genero_id";

            var resultadoFrutas = await conexion
                .QueryAsync<FrutaClasificada>(sentenciaSQL, parametrosSentencia);

            if (resultadoFrutas.Any())
            {
                frutasClasificadas = resultadoFrutas.ToList();

                foreach (FrutaClasificada unaFruta in frutasClasificadas.ToList())
                    unaFruta.Taxonomia = await GetTaxonomicDetailsAsync(unaFruta.Id);
            }

            return frutasClasificadas;
        }

        public async Task<FrutaDetallada> GetDetailsByIdAsync(string? fruta_id)
        {
            Fruta unaFruta = await GetByIdAsync(fruta_id);

            FrutaDetallada unaFrutaDetallada = new()
            {
                Id = unaFruta.Id,
                Nombre = unaFruta.Nombre,
                Url_Imagen = unaFruta.Url_Imagen,
                Url_Wikipedia = unaFruta.Url_Wikipedia,
                Produccion = await GetProductionDetailsAsync(fruta_id, string?.Empty, string?.Empty),
                Nutricion = await GetNutritionDetailsAsync(fruta_id),
                Taxonomia = await GetTaxonomicDetailsAsync(fruta_id)
            };

            return unaFrutaDetallada;
        }

        public async Task<List<Produccion>> GetProductionDetailsAsync(string? fruta_id, string? departamento_id, string? municipio_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.string?, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento, " +
                "v.mes_inicio, v.mes_final " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id ";

            if (departamento_id != string?.Empty)
            {
                parametrosSentencia.Add("@departamento_id", departamento_id,
                DbType.string?, ParameterDirection.Input);

                sentenciaSQL += "AND v.departamento_id = @departamento_id";
            }

            if (municipio_id != string?.Empty)
            {
                parametrosSentencia.Add("@municipio_id", municipio_id,
                DbType.string?, ParameterDirection.Input);

                sentenciaSQL += "AND v.municipio_id = @municipio_id";
            }

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultadoProduccion.Any())
                infoProduccion = resultadoProduccion.ToList();

            return infoProduccion;
        }

        public async Task<List<Produccion>> GetProductionDetailsAsync(string? fruta_id, string? clima_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.string?, ParameterDirection.Input);

            parametrosSentencia.Add("@clima_id", clima_id,
                                    DbType.string?, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento, " +
                "v.mes_inicio, v.mes_final " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id " +
                "AND v.clima_id = @clima_id";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultadoProduccion.Any())
                infoProduccion = resultadoProduccion.ToList();

            return infoProduccion;
        }

        public async Task<List<Produccion>> GetProductionDetailsAsync(string? fruta_id, int mes_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.string?, ParameterDirection.Input);

            parametrosSentencia.Add("@mes_id", mes_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento, " +
                "v.mes_inicio, v.mes_final " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id " +
                "AND @mes_id BETWEEN mes_inicio and mes_final";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultadoProduccion.Any())
                infoProduccion = resultadoProduccion.ToList();

            return infoProduccion;
        }

        public async Task<Nutricion> GetNutritionDetailsAsync(string? fruta_id)
        {
            Nutricion infoNutricion = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.string?, ParameterDirection.Input);

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

        public async Task<FrutaNutritiva> GetNutritiousFruitByIdAsync(string? fruta_id)
        {
            Fruta unaFruta = await GetByIdAsync(fruta_id);

            FrutaNutritiva unaFrutaNutritiva = new()
            {
                Id = unaFruta.Id,
                Nombre = unaFruta.Nombre,
                Url_Imagen = unaFruta.Url_Imagen,
                Url_Wikipedia = unaFruta.Url_Wikipedia,
                Nutricion = await GetNutritionDetailsAsync(fruta_id)
            };

            return unaFrutaNutritiva;
        }

        public async Task<FrutaClasificada> GetClassifiedFruitByIdAsync(string? fruta_id)
        {
            Fruta unaFruta = await GetByIdAsync(fruta_id);

            FrutaClasificada unafrutaClasificada = new()
            {
                Id = unaFruta.Id,
                Nombre = unaFruta.Nombre,
                Url_Imagen = unaFruta.Url_Imagen,
                Url_Wikipedia = unaFruta.Url_Wikipedia,
                Taxonomia = await GetTaxonomicDetailsAsync(fruta_id)
            };

            return unafrutaClasificada;
        }

        public async Task<Taxonomia> GetTaxonomicDetailsAsync(string? fruta_id)
        {
            Taxonomia infoTaxonomia = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.string?, ParameterDirection.Input);

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

            //Validamos si la fruta existe por ese Nombre
            var frutaExistente = await GetByNameAsync(unaFruta.Nombre!);

            if (frutaExistente.Id != string?.Empty)
                throw new DbOperationException($"No se puede insertar. Ya existe la fruta {unaFruta.Nombre!}.");

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

        public async Task<bool> CreateNutritionDetailsAsync(string? fruta_id, Nutricion unaInformacionNutricional)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalNutritionDetailsByFruitIdAsync(fruta_id);

            if (totalRegistros != 0)
                throw new DbOperationException("No se puede insertar. Ya existen registros nutricionales para esta fruta.");

            try
            {
                var conexion = contextoDB.CreateConnection();
                string procedimiento = "core.p_inserta_nutricion_fruta";
                var parametros = new
                {
                    p_fruta_id = fruta_id,
                    p_azucares = unaInformacionNutricional.Azucares,
                    p_carbohidratos = unaInformacionNutricional.Carbohidratos,
                    p_grasas = unaInformacionNutricional.Grasas,
                    p_proteinas = unaInformacionNutricional.Proteinas
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

        public async Task<bool> CreateTaxonomicDetailsAsync(string? fruta_id, Taxonomia unaInformacionTaxonomica)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalTaxonomicDetailsByFruitIdAsync(fruta_id);

            if (totalRegistros != 0)
                throw new DbOperationException("No se puede insertar. Ya existen registros taxonómicos para esta fruta.");

            try
            {
                var conexion = contextoDB.CreateConnection();
                string procedimiento = "core.p_inserta_taxonomia_fruta";
                var parametros = new
                {
                    p_fruta_id = fruta_id,
                    p_reino = unaInformacionTaxonomica.Reino,
                    p_division = unaInformacionTaxonomica.Division,
                    p_clase = unaInformacionTaxonomica.Clase,
                    p_orden = unaInformacionTaxonomica.Orden,
                    p_familia = unaInformacionTaxonomica.Familia,
                    p_genero = unaInformacionTaxonomica.Genero,
                    p_especie = unaInformacionTaxonomica.Especie
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

            //Validamos si la fruta existe por ese ID
            var frutaExistente = await GetByIdAsync(unaFruta.Id);

            if (frutaExistente.Id == string?.Empty)
                throw new DbOperationException($"No se puede actualizar. No existe la fruta {unaFruta.Nombre!}.");

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

        public async Task<bool> UpdateNutritionDetailsAsync(string? fruta_id, Nutricion unaInformacionNutricional)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalNutritionDetailsByFruitIdAsync(fruta_id);

            if (totalRegistros == 0)
                throw new DbOperationException("No se puede actualizar. NO existen registros nutricionales para esta fruta.");

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_nutricion_fruta";
                var parametros = new
                {
                    p_fruta_id = fruta_id,
                    p_azucares = unaInformacionNutricional.Azucares,
                    p_carbohidratos = unaInformacionNutricional.Carbohidratos,
                    p_grasas = unaInformacionNutricional.Grasas,
                    p_proteinas = unaInformacionNutricional.Proteinas
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

        public async Task<bool> UpdateTaxonomicDetailsAsync(string? fruta_id, Taxonomia unaInformacionTaxonomica)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalTaxonomicDetailsByFruitIdAsync(fruta_id);

            if (totalRegistros == 0)
                throw new DbOperationException("No se puede actualizar. NO existen registros taxonómicos para esta fruta.");

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_actualiza_taxonomia_fruta";
                var parametros = new
                {
                    p_fruta_id = fruta_id,
                    p_reino = unaInformacionTaxonomica.Reino,
                    p_division = unaInformacionTaxonomica.Division,
                    p_clase = unaInformacionTaxonomica.Clase,
                    p_orden = unaInformacionTaxonomica.Orden,
                    p_familia = unaInformacionTaxonomica.Familia,
                    p_genero = unaInformacionTaxonomica.Genero,
                    p_especie = unaInformacionTaxonomica.Especie
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

        public async Task<bool> RemoveAsync(string? fruta_id)
        {
            bool resultadoAccion = false;

            //Validamos si la fruta existe por ese ID
            var frutaExistente = await GetByIdAsync(fruta_id);

            if (frutaExistente.Id == string?.Empty)
                throw new DbOperationException($"No se puede eliminar. No existe la fruta con el Id {fruta_id}.");

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

        public async Task<bool> RemoveNutritionDetailsAsync(string? fruta_id)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalNutritionDetailsByFruitIdAsync(fruta_id);

            if (totalRegistros == 0)
                throw new DbOperationException("No se puede eliminar. NO existen registros nutricionales para esta fruta.");


            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_nutricion_fruta";
                var parametros = new
                {
                    p_fruta_id = fruta_id
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

        public async Task<bool> RemoveTaxonomicDetailsAsync(string? fruta_id)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalTaxonomicDetailsByFruitIdAsync(fruta_id);

            if (totalRegistros == 0)
                throw new DbOperationException("No se puede eliminar. NO existen registros taxonómicos para esta fruta.");


            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_taxonomia_fruta";
                var parametros = new
                {
                    p_fruta_id = fruta_id
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

        private async Task<int> GetTotalNutritionDetailsByFruitIdAsync(string? fruta_id)
        {
            var conexion = contextoDB.CreateConnection();

            //Validamos si hay registros existentes
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.string?, ParameterDirection.Input);

            //Aqui colocamos la informacion nutricional
            string sentenciaSQL = "SELECT count(*) totalRegistros " +
                "FROM core.nutricion_frutas " +
                "WHERE fruta_id = @fruta_id";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalRegistros.FirstOrDefault();
        }

        private async Task<int> GetTotalTaxonomicDetailsByFruitIdAsync(string? fruta_id)
        {
            var conexion = contextoDB.CreateConnection();

            //Validamos si hay registros existentes
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.string?, ParameterDirection.Input);

            //Aqui colocamos la informacion nutricional
            string sentenciaSQL = "SELECT count(*) totalRegistros " +
                "FROM core.taxonomia_frutas " +
                "WHERE fruta_id = @fruta_id";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalRegistros.FirstOrDefault();
        }
    }
}
