﻿using Dapper;
using FrutasColombia_CS_REST_API.DbContexts;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;
using System.Data;

namespace FrutasColombia_CS_REST_API.Repositories
{
    public class DepartamentoRepository(PgsqlDbContext unContexto) : IDepartamentoRepository
    {
        private readonly PgsqlDbContext contextoDB = unContexto;

        public async Task<IEnumerable<Departamento>> GetAllAsync()
        {
            var conexion = contextoDB.CreateConnection();

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM core.departamentos " +
                                  "ORDER BY nombre";

            var resultadoDepartamentos = await conexion
                .QueryAsync<Departamento>(sentenciaSQL, new DynamicParameters());

            return resultadoDepartamentos;
        }

        public async Task<Departamento> GetByIdAsync(string departamento_id)
        {
            Departamento unDepartamento = new();

            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@departamento_id", departamento_id,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre " +
                                  "FROM core.departamentos " +
                                  "WHERE id = @departamento_id ";

            var resultado = await conexion.QueryAsync<Departamento>(sentenciaSQL,
                parametrosSentencia);

            if (resultado.Any())
                unDepartamento = resultado.First();

            return unDepartamento;
        }

        public async Task<IEnumerable<Municipio>> GetAssociatedMunicipiosAsync(string departamento_id)
        {
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@departamento_id", departamento_id,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT id, nombre, departamento_id " +
                                  "FROM core.municipios " +
                                  "WHERE departamento_id = @departamento_id " +
                                  "ORDER BY nombre";

            var resultadoMunicipios = await conexion
                .QueryAsync<Municipio>(sentenciaSQL, parametrosSentencia);

            return resultadoMunicipios;
        }

    }
}
