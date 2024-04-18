﻿using FrutasColombia_CS_NoSQL_REST_API.DbContexts;
using FrutasColombia_CS_NoSQL_REST_API.Interfaces;
using FrutasColombia_CS_NoSQL_REST_API.Models;
using System.Data;

namespace FrutasColombia_CS_NoSQL_REST_API.Repositories
{
    public class ClimaRepository(MongoDbContext unContexto) : IClimaRepository
    {
        private readonly MongoDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Clima>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT id, nombre, altitud_minima, altitud_maxima " +
                                  "FROM core.climas " +
                                  "ORDER BY id";

            var resultadoClimas = await conexion
                .QueryAsync<Clima>(sentenciaSQL, new DynamicParameters());

            return resultadoClimas;
        }

        public async Task<Clima> GetByIdAsync(Guid clima_id)
        {
            Clima unClima = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@clima_id", clima_id,
                                    DbType.Guid, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, altitud_minima, altitud_maxima " +
                                  "FROM core.climas " +
                                  "WHERE id = @clima_id";

            var resultado = await conexion.QueryAsync<Clima>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unClima = resultado.First();

            return unClima;
        }
    }
}