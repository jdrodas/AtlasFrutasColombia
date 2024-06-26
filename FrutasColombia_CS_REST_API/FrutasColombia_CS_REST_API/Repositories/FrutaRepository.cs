﻿using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Exceptions;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using Npgsql;
using System.Data;


namespace FrutasColombia_CS_REST_API.Repositories
{
    public class FrutaRepository(PgsqlDbContext unContexto) : IFrutaRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<List<Fruta>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                  "FROM core.frutas " +
                                  "ORDER BY nombre";

            var resultadoFrutas = await conexion
                .QueryAsync<Fruta>(sentenciaSQL, new DynamicParameters());

            return resultadoFrutas.ToList();
        }

        public async Task<Fruta> GetByIdAsync(Guid fruta_id)
        {
            Fruta unaFruta = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);

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

        public async Task<List<FrutaClasificada>> GetClassifiedByGenusIdAsync(Guid genero_id)
        {
            List<FrutaClasificada> frutasClasificadas = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@genero_id", genero_id,
                DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT fruta_id id, fruta_nombre nombre, " +
                "url_wikipedia, url_imagen " +
                "FROM v_info_frutas v " +
                "WHERE genero_id = @genero_id";

            var resultado = await conexion
                .QueryAsync<FrutaClasificada>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
            {
                frutasClasificadas = resultado.ToList();

                foreach (FrutaClasificada unaFruta in frutasClasificadas)
                    unaFruta.Taxonomia = await GetTaxonomicDetailsAsync(unaFruta.Id);
            }

            return frutasClasificadas;
        }

        public async Task<FrutaDetallada> GetDetailsByIdAsync(Guid fruta_id)
        {
            Fruta unaFruta = await GetByIdAsync(fruta_id);

            FrutaDetallada unaFrutaDetallada = new()
            {
                Id = unaFruta.Id,
                Nombre = unaFruta.Nombre,
                Url_Imagen = unaFruta.Url_Imagen,
                Url_Wikipedia = unaFruta.Url_Wikipedia,
                Produccion = await GetProductionDetailsAsync(fruta_id, Guid.Empty, Guid.Empty),
                Nutricion = await GetNutritionDetailsAsync(fruta_id),
                Taxonomia = await GetTaxonomicDetailsAsync(fruta_id)
            };

            return unaFrutaDetallada;
        }

        public async Task<bool> CreateAsync(Fruta unaFruta)
        {
            bool resultadoAccion = false;

            //Validamos si la fruta existe por ese Nombre
            var frutaExistente = await GetByNameAsync(unaFruta.Nombre!);

            if (frutaExistente.Id != Guid.Empty)
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

        public async Task<bool> UpdateAsync(Fruta unaFruta)
        {
            bool resultadoAccion = false;

            //Validamos si la fruta existe por ese ID
            var frutaExistente = await GetByIdAsync(unaFruta.Id);

            if (frutaExistente.Id == Guid.Empty)
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

        public async Task<bool> RemoveAsync(Guid fruta_id)
        {
            bool resultadoAccion = false;

            //Validamos si la fruta existe por ese ID
            var frutaExistente = await GetByIdAsync(fruta_id);

            if (frutaExistente.Id == Guid.Empty)
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

        #region produccion_frutas

        public async Task<List<FrutaProducida>> GetProducedByLocationAsync(Guid departamento_id, Guid municipio_id)
        {
            List<FrutaProducida> frutasProducidas = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            string sentenciaSQL = "SELECT DISTINCT fruta_id id, fruta_nombre nombre, " +
                "fruta_wikipedia url_wikipedia, fruta_imagen url_imagen " +
                "FROM v_info_produccion_frutas v ";

            if (departamento_id != Guid.Empty)
            {
                parametrosSentencia.Add("@departamento_id", departamento_id,
                DbType.Guid, ParameterDirection.Input);

                sentenciaSQL += "WHERE v.departamento_id = @departamento_id";
            }

            if (municipio_id != Guid.Empty)
            {
                parametrosSentencia.Add("@municipio_id", municipio_id,
                DbType.Guid, ParameterDirection.Input);

                sentenciaSQL += "WHERE v.municipio_id = @municipio_id";
            }

            var resultado = await conexion
                .QueryAsync<FrutaProducida>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
            {
                frutasProducidas = resultado.ToList();

                foreach (FrutaProducida unaFruta in frutasProducidas)
                {
                    if (departamento_id != Guid.Empty)
                        unaFruta.Produccion = await GetProductionDetailsAsync(unaFruta.Id, departamento_id, Guid.Empty);

                    if (municipio_id != Guid.Empty)
                        unaFruta.Produccion = await GetProductionDetailsAsync(unaFruta.Id, Guid.Empty, municipio_id);
                }
            }

            return frutasProducidas;
        }

        public async Task<List<FrutaProducida>> GetProducedByClimateAsync(Guid clima_id)
        {
            List<FrutaProducida> frutasProducidas = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@clima_id", clima_id,
                DbType.Guid, ParameterDirection.Input);


            string sentenciaSQL = "SELECT DISTINCT fruta_id id, fruta_nombre nombre, " +
                "fruta_wikipedia url_wikipedia, fruta_imagen url_imagen " +
                "FROM v_info_produccion_frutas v " +
                "WHERE clima_id = @clima_id";

            var resultado = await conexion
                .QueryAsync<FrutaProducida>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
            {
                frutasProducidas = resultado.ToList();

                foreach (FrutaProducida unaFruta in frutasProducidas)
                    unaFruta.Produccion = await GetProductionDetailsAsync(unaFruta.Id, clima_id);
            }

            return frutasProducidas;
        }

        public async Task<List<FrutaProducida>> GetProducedByEpochAsync(Guid epoca_id)
        {
            List<FrutaProducida> frutasProducidas = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@epoca_id", epoca_id,
                DbType.Guid, ParameterDirection.Input);


            string sentenciaSQL = "SELECT DISTINCT fruta_id id, fruta_nombre nombre, " +
                "fruta_wikipedia url_wikipedia, fruta_imagen url_imagen " +
                "FROM v_info_produccion_frutas v " +
                "WHERE epoca_id = @epoca_id";

            var resultado = await conexion
                .QueryAsync<FrutaProducida>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
            {
                frutasProducidas = resultado.ToList();

                foreach (FrutaProducida unaFruta in frutasProducidas)
                    unaFruta.Produccion = await GetEpochProductionDetailsAsync(unaFruta.Id, epoca_id);
            }

            return frutasProducidas;
        }

        public async Task<List<FrutaProducida>> GetProducedByMonthAsync(int mes_id)
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

            var resultado = await conexion
                .QueryAsync<FrutaProducida>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
            {
                frutasProducidas = resultado.ToList();

                foreach (FrutaProducida unaFruta in frutasProducidas)
                    unaFruta.Produccion = await GetProductionDetailsAsync(unaFruta.Id, mes_id);
            }

            return frutasProducidas;
        }

        public async Task<FrutaProducida> GetProducedFruitByIdAsync(Guid fruta_id)
        {
            Fruta unaFruta = await GetByIdAsync(fruta_id);

            FrutaProducida unafrutaClasificada = new()
            {
                Id = unaFruta.Id,
                Nombre = unaFruta.Nombre,
                Url_Imagen = unaFruta.Url_Imagen,
                Url_Wikipedia = unaFruta.Url_Wikipedia,
                Produccion = await GetProductionDetailsAsync(fruta_id)
            };

            return unafrutaClasificada;
        }

        public async Task<List<Produccion>> GetProductionDetailsAsync(Guid fruta_id, Guid departamento_id, Guid municipio_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento, " +
                "v.mes_inicio, v.mes_final " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id ";

            if (departamento_id != Guid.Empty)
            {
                parametrosSentencia.Add("@departamento_id", departamento_id,
                DbType.Guid, ParameterDirection.Input);

                sentenciaSQL += "AND v.departamento_id = @departamento_id";
            }

            if (municipio_id != Guid.Empty)
            {
                parametrosSentencia.Add("@municipio_id", municipio_id,
                DbType.Guid, ParameterDirection.Input);

                sentenciaSQL += "AND v.municipio_id = @municipio_id";
            }

            var resultado = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                infoProduccion = resultado.ToList();

            return infoProduccion;
        }

        public async Task<List<Produccion>> GetProductionDetailsAsync(Guid fruta_id, Guid clima_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);

            parametrosSentencia.Add("@clima_id", clima_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento, " +
                "v.mes_inicio, v.mes_final " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id " +
                "AND v.clima_id = @clima_id";

            var resultado = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                infoProduccion = resultado.ToList();

            return infoProduccion;
        }

        public async Task<List<Produccion>> GetEpochProductionDetailsAsync(Guid fruta_id, Guid epoca_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);

            parametrosSentencia.Add("@epoca_id", epoca_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento, " +
                "v.mes_inicio, v.mes_final " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id " +
                "AND v.epoca_id = @epoca_id";

            var resultado = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                infoProduccion = resultado.ToList();

            return infoProduccion;
        }

        public async Task<List<Produccion>> GetProductionDetailsAsync(Guid fruta_id, int mes_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);

            parametrosSentencia.Add("@mes_id", mes_id,
                                    DbType.Int32, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento, " +
                "v.mes_inicio, v.mes_final " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id " +
                "AND @mes_id BETWEEN mes_inicio and mes_final";

            var resultado = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                infoProduccion = resultado.ToList();

            return infoProduccion;
        }

        public async Task<List<Produccion>> GetProductionDetailsAsync(Guid fruta_id)
        {
            List<Produccion> infoProduccion = [];

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);


            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento, " +
                "v.mes_inicio, v.mes_final " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id ";

            var resultado = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                infoProduccion = resultado.ToList();

            return infoProduccion;
        }

        private async Task<int> GetTotalProductionDetailsByFruitIdAsync(Guid fruta_id, Produccion unaInformacionProductiva)
        {
            var conexion = contextoDB.CreateConnection();

            //Validamos si hay registros existentes
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);
            parametrosSentencia.Add("@epoca_nombre", unaInformacionProductiva.Epoca,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@clima_nombre", unaInformacionProductiva.Clima,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@municipio_nombre", unaInformacionProductiva.Municipio,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@departamento_nombre", unaInformacionProductiva.Departamento,
                                    DbType.String, ParameterDirection.Input);
            parametrosSentencia.Add("@mes_inicio", unaInformacionProductiva.Mes_Inicio,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@mes_final", unaInformacionProductiva.Mes_Final,
                                    DbType.Int32, ParameterDirection.Input);

            //Aqui colocamos la informacion nutricional
            string sentenciaSQL = "SELECT count(*) totalRegistros " +
                "FROM core.v_info_produccion_frutas " +
                "WHERE fruta_id = @fruta_id " +
                "AND epoca_nombre = @epoca_nombre " +
                "AND clima_nombre = @clima_nombre " +
                "AND municipio_nombre = @municipio_nombre " +
                "AND departamento_nombre = @departamento_nombre " +
                "AND mes_inicio = @mes_inicio " +
                "AND mes_final = @mes_final";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalRegistros.FirstOrDefault();
        }

        public async Task<bool> CreateProductionDetailsAsync(Guid fruta_id, Produccion unaInformacionProductiva)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalProductionDetailsByFruitIdAsync(fruta_id, unaInformacionProductiva);

            if (totalRegistros != 0)
                throw new DbOperationException("No se puede insertar. Ya se ha registrado previamente esta información de producción.");

            try
            {
                var conexion = contextoDB.CreateConnection();
                string procedimiento = "core.p_inserta_produccion_fruta";
                var parametros = new
                {
                    p_fruta_id = fruta_id,
                    p_epoca_nombre = unaInformacionProductiva.Epoca,
                    p_clima_nombre = unaInformacionProductiva.Clima,
                    p_municipio_nombre = unaInformacionProductiva.Municipio,
                    p_departamento_nombre = unaInformacionProductiva.Departamento,
                    p_mes_inicio = unaInformacionProductiva.Mes_Inicio,
                    p_mes_final = unaInformacionProductiva.Mes_Final
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

        public async Task<bool> RemoveProductionDetailsAsync(Guid fruta_id, Produccion unaInformacionProductiva)
        {
            bool resultadoAccion = false;

            //Validamos si hay registros existentes
            int totalRegistros =
                await GetTotalProductionDetailsByFruitIdAsync(fruta_id, unaInformacionProductiva);

            if (totalRegistros == 0)
                throw new DbOperationException("No se puede eliminar. Esta información de producción no está registrada previamente.");

            try
            {
                var conexion = contextoDB.CreateConnection();

                string procedimiento = "core.p_elimina_produccion_fruta";
                var parametros = new
                {
                    p_fruta_id = fruta_id,
                    p_epoca_nombre = unaInformacionProductiva.Epoca,
                    p_clima_nombre = unaInformacionProductiva.Clima,
                    p_municipio_nombre = unaInformacionProductiva.Municipio,
                    p_departamento_nombre = unaInformacionProductiva.Departamento,
                    p_mes_inicio = unaInformacionProductiva.Mes_Inicio,
                    p_mes_final = unaInformacionProductiva.Mes_Final
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

        #endregion produccion_frutas

        #region nutricion_frutas

        public async Task<FrutaNutritiva> GetNutritiousFruitByIdAsync(Guid fruta_id)
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

        private async Task<int> GetTotalNutritionDetailsByFruitIdAsync(Guid fruta_id)
        {
            var conexion = contextoDB.CreateConnection();

            //Validamos si hay registros existentes
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);

            //Aqui colocamos la informacion nutricional
            string sentenciaSQL = "SELECT count(*) totalRegistros " +
                "FROM core.nutricion_frutas " +
                "WHERE fruta_id = @fruta_id";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalRegistros.FirstOrDefault();
        }

        public async Task<Nutricion> GetNutritionDetailsAsync(Guid fruta_id)
        {
            Nutricion infoNutricion = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);

            //Aqui colocamos la informacion nutricional
            string sentenciaSQL = "SELECT DISTINCT azucares, carbohidratos, grasas, proteinas " +
                "FROM core.v_info_nutricion_frutas v " +
                "WHERE v.fruta_id = @fruta_id";

            var resultado = await conexion
                .QueryAsync<Nutricion>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                infoNutricion = resultado.First();

            return infoNutricion;
        }

        public async Task<bool> CreateNutritionDetailsAsync(Guid fruta_id, Nutricion unaInformacionNutricional)
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

        public async Task<bool> UpdateNutritionDetailsAsync(Guid fruta_id, Nutricion unaInformacionNutricional)
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

        public async Task<bool> RemoveNutritionDetailsAsync(Guid fruta_id)
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

        #endregion nutricion_frutas

        #region taxonomia_frutas

        public async Task<FrutaClasificada> GetClassifiedFruitByIdAsync(Guid fruta_id)
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

        private async Task<int> GetTotalTaxonomicDetailsByFruitIdAsync(Guid fruta_id)
        {
            var conexion = contextoDB.CreateConnection();

            //Validamos si hay registros existentes
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);

            //Aqui colocamos la informacion nutricional
            string sentenciaSQL = "SELECT count(*) totalRegistros " +
                "FROM core.taxonomia_frutas " +
                "WHERE fruta_id = @fruta_id";

            var totalRegistros = await conexion
                .QueryAsync<int>(sentenciaSQL, parametrosSentencia);

            return totalRegistros.FirstOrDefault();
        }

        public async Task<Taxonomia> GetTaxonomicDetailsAsync(Guid fruta_id)
        {
            Taxonomia infoTaxonomia = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Guid, ParameterDirection.Input);

            //Aqui colocamos la informacion de clasificación taxonómica
            string sentenciaSQL = "SELECT DISTINCT reino_nombre reino, division_nombre division, clase_nombre clase, " +
                "orden_nombre orden, familia_nombre familia, " +
                "genero_nombre genero, especie_nombre especie " +
                "FROM core.v_info_frutas v " +
                "WHERE v.fruta_id = @fruta_id";

            var resultado = await conexion
                .QueryAsync<Taxonomia>(sentenciaSQL, parametrosSentencia);

            if (resultado.Any())
                infoTaxonomia = resultado.First();

            return infoTaxonomia;
        }

        public async Task<bool> CreateTaxonomicDetailsAsync(Guid fruta_id, Taxonomia unaInformacionTaxonomica)
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

        public async Task<bool> UpdateTaxonomicDetailsAsync(Guid fruta_id, Taxonomia unaInformacionTaxonomica)
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

        public async Task<bool> RemoveTaxonomicDetailsAsync(Guid fruta_id)
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

        #endregion taxonomia_frutas
    }
}
