﻿using Dapper;
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
