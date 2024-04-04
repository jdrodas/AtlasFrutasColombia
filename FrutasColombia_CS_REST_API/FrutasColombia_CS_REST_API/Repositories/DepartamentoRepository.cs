using Dapper;
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

        public async Task<IEnumerable<Municipio>> GetAssociatedMunicipalityAsync(string departamento_id)
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

        public async Task<IEnumerable<FrutaProducida>> GetProducedFruitsAsync(string departamento_id)
        {


            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@departamento_id", departamento_id,
                                    DbType.String, ParameterDirection.Input);

            string sentenciaSQL = "SELECT DISTINCT fruta_id id, fruta_nombre nombre, " +
                "fruta_wikipedia url_wikipedia, fruta_imagen url_imagen " +
                "FROM v_info_produccion_frutas v " +
                "WHERE departamento_id = @departamento_id ";

            var resultadoFrutasDepartamento = await conexion
                .QueryAsync<FrutaProducida>(sentenciaSQL, parametrosSentencia);

            if (resultadoFrutasDepartamento.Any())
            {
                foreach (FrutaProducida unaFruta in resultadoFrutasDepartamento.ToList())
                {
                    unaFruta.Produccion = await GetFruitProductionDetails(unaFruta.Id, departamento_id);
                }
            }

            return resultadoFrutasDepartamento.ToList();

        }

        public async Task<List<Produccion>> GetFruitProductionDetails(int fruta_id, string departamento_id)
        {
            List<Produccion> infoProduccion = new();
            
            var conexion = contextoDB.CreateConnection();

            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@fruta_id", fruta_id,
                                    DbType.Int32, ParameterDirection.Input);
            parametrosSentencia.Add("@departamento_id", departamento_id,
                        DbType.String, ParameterDirection.Input);

            //Aqui obtenemos la informacion de producción
            string sentenciaSQL = "SELECT DISTINCT v.epoca_nombre epoca, v.clima_nombre clima, " +
                "v.municipio_nombre municipio, v.departamento_nombre departamento " +
                "FROM v_info_produccion_frutas v " +
                "WHERE v.fruta_id = @fruta_id " +
                "AND v.departamento_id = @departamento_id";

            var resultadoProduccion = await conexion
                .QueryAsync<Produccion>(sentenciaSQL, parametrosSentencia);

            if (resultadoProduccion.Any())
                infoProduccion = resultadoProduccion.ToList();

            return infoProduccion;
        }
    }
}
