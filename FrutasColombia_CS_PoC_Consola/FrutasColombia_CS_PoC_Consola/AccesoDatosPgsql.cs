using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace FrutasColombia_CS_PoC_Consola
{
    public class AccesoDatosPgsql
    {
        public static string? ObtieneCadenaConexion()
        {
            //Parametrizamos el acceso al archivo de configuración appsettings.json
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration miConfiguracion = builder.Build();

            return miConfiguracion["ConnectionString:FrutasPL"];
        }

        #region Frutas

        /// <summary>
        /// Obtiene la lista con los nombres de las frutas
        /// </summary>
        /// <returns>Lista con los nombres de las frutas</returns>
        public static List<string> ObtieneNombresFrutas()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            using IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion);
            string sentenciaSQL = "SELECT nombre FROM frutas ORDER BY nombre";
            var resultadoFrutas = cxnDB.Query<string>(sentenciaSQL, new DynamicParameters());

            return resultadoFrutas.AsList();
        }

        /// <summary>
        /// Obtiene la lista con las frutas del atlas
        /// </summary>
        /// <returns></returns>
        public static List<Fruta> ObtieneListaFrutas()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            using IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion);
            string sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen FROM frutas ORDER BY nombre";
            var resultadoFrutas = cxnDB.Query<Fruta>(sentenciaSQL, new DynamicParameters());

            return resultadoFrutas.AsList();
        }

        /// <summary>
        /// Obtiene una fruta según el Id
        /// </summary>
        /// <param name="idFruta">ID de la fruta a buscar</param>
        /// <returns>La fruta identificada según el parámetro</returns>
        public static Fruta ObtieneFruta(int idFruta)
        {
            Fruta frutaResultado = new();
            string? cadenaConexion = ObtieneCadenaConexion();

            //Aqui buscamos la fruta asociada al nombre
            using IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion);
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@id_fruta", idFruta,
                                    DbType.Int32, ParameterDirection.Input);

            string? sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                    "FROM frutas " +
                                    "WHERE id = @id_fruta";

            var salida = cxnDB.Query<Fruta>(sentenciaSQL, parametrosSentencia);

            if (salida.Any())
                frutaResultado = salida.First();

            return frutaResultado;
        }

        /// <summary>
        /// Obtiene una fruta según el nombre
        /// </summary>
        /// <param name="nombreFruta">Nombre de la fruta a buscar</param>
        /// <returns>La fruta identificada según el parámetro</returns>
        public static Fruta ObtieneFruta(string nombreFruta)
        {
            Fruta frutaResultado = new();
            string? cadenaConexion = ObtieneCadenaConexion();

            //Aqui buscamos la fruta asociada al nombre
            using IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion);
            DynamicParameters parametrosSentencia = new();
            parametrosSentencia.Add("@nombre_fruta", nombreFruta,
                                    DbType.String, ParameterDirection.Input);

            string? sentenciaSQL = "SELECT id, nombre, url_wikipedia, url_imagen " +
                                    "FROM frutas " +
                                    "WHERE nombre = @nombre_fruta";

            var salida = cxnDB.Query<Fruta>(sentenciaSQL, parametrosSentencia);

            if (salida.Any())
                frutaResultado = salida.First();

            return frutaResultado;
        }

        /// <summary>
        /// Inserta una fruta
        /// </summary>
        /// <param name="unaFruta">La fruta a insertar</param>
        /// <returns>Verdadero si la inserción se hizo correctamente</returns>
        public static bool InsertaFruta(Fruta unaFruta)
        {
            //Validaciones previas: 
            // - Que la fruta no exista previamente

            int cantidadFilas;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();

            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                DynamicParameters parametrosSentencia = new ();
                parametrosSentencia.Add("@nombre_fruta", unaFruta.Nombre,
                    DbType.String, ParameterDirection.Input);

                //Preguntamos si ya existe una fruta con ese nombre
                string consultaSQL = "SELECT COUNT(id) total " +
                                           "FROM frutas " +
                                           "WHERE LOWER(nombre) = LOWER(@nombre_fruta)";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                // Si hay filas, ya existe una fruta con ese nombre
                if (cantidadFilas != 0)
                    return false;

                try
                {
                    string insertaFrutaSQL = "INSERT INTO frutas (nombre, url_wikipedia, url_imagen) " +
                                               "VALUES (@Nombre, @Url_Wikipedia, @Url_Imagen)";

                    cantidadFilas = cxnDB.Execute(insertaFrutaSQL, unaFruta);
                }
                catch (NpgsqlException)
                {
                    resultado = false;
                    cantidadFilas = 0;
                }

                //Si la inserción fue correcta, se afectaron filas y podemos retornar true.
                if (cantidadFilas > 0)
                    resultado = true;

            }

            return resultado;
        }

        /// <summary>
        /// Actualiza la información básica de una fruta
        /// </summary>
        /// <param name="frutaActualizada">El objeto fruta para actualizar</param>
        /// <returns>Verdadero si la actualización se hizo correctamente</returns>
        public static bool ActualizaFruta(Fruta frutaActualizada)
        {
            //Validaciones previas: 
            // - Que la a actualizar exista - Busqueda por ID
            // - Que el nombre nuevo no exista previamente

            int cantidadFilas;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();


            using (IDbConnection cxnDB = new NpgsqlConnection(cadenaConexion))
            {
                //Aqui validamos primero que la fruta previamente existe

                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@fruta_id", frutaActualizada.Id,
                                        DbType.Int32, ParameterDirection.Input);

                string consultaSQL = "SELECT COUNT(id) total " +
                                     "FROM frutas " +
                                     "WHERE id = @fruta_id";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si no hay filas, no existe fruta que actualizar
                if (cantidadFilas == 0)
                    return false;

                //Aqui validamos que no exista frutas con el nuevo nombre
                parametrosSentencia = new DynamicParameters();
                parametrosSentencia.Add("@fruta_nombre", frutaActualizada.Nombre,
                                        DbType.String, ParameterDirection.Input);

                //Validamos si el nuevo nombre no exista
                consultaSQL = "SELECT COUNT(id) total " +
                              "FROM frutas " +
                              "WHERE nombre = @fruta_nombre";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si hay filas, el nuevo nombre a utilizar ya existe!
                if (cantidadFilas != 0)
                    return false;

                //Terminadas las validaciones, realizamos el update
                try
                {
                    string actualizaFrutasSql = "UPDATE frutas SET nombre = @Nombre, url_wikipedia = @Url_Wikipedia, url_imagen = @Url_Imagen " +
                        "WHERE id = @Id"; ;

                    //Aqui no usamos parámetros dinámicos, pasamos el objeto!!!
                    cantidadFilas = cxnDB.Execute(actualizaFrutasSql, frutaActualizada);
                }
                catch (NpgsqlException)
                {
                    resultado = false;
                    cantidadFilas = 0;
                }

                //Si la actualización fue correcta, devolvemos true
                if (cantidadFilas > 0)
                    resultado = true;
            }

            return resultado;
        }

        /// <summary>
        /// Elimina una fruta existente
        /// </summary>
        /// <param name="unaFruta">Objeto fruta a eliminar</param>
        /// <returns>Verdadero si la eliminación se hizo correctamente</returns>
        public static bool EliminaFruta(Fruta unaFruta, out string mensajeEliminacion)
        {
            //Validaciones previas: 
            // - Que la fruta a actualizar exista - Busqueda por ID

            mensajeEliminacion = string.Empty;
            int cantidadFilas;
            bool resultado = false;
            string? cadenaConexion = ObtieneCadenaConexion();

            using (NpgsqlConnection cxnDB = new(cadenaConexion))
            {
                //Primero, identificamos si hay una fruta con este nombre y ese Id

                DynamicParameters parametrosSentencia = new();
                parametrosSentencia.Add("@fruta_nombre", unaFruta.Nombre,
                                        DbType.String, ParameterDirection.Input);

                parametrosSentencia.Add("@fruta_id", unaFruta.Id,
                                        DbType.Int32, ParameterDirection.Input);                                        

                string consultaSQL = "SELECT COUNT(id) total " +
                                     "FROM frutas " +
                                     "WHERE nombre = @fruta_nombre and id = @fruta_id";

                cantidadFilas = cxnDB.Query<int>(consultaSQL, parametrosSentencia).FirstOrDefault();

                //Si no hay filas, no existe una fruta con ese nombre y ese Id ... no hay nada que eliminar.
                if (cantidadFilas == 0)
                {
                    mensajeEliminacion = $"Eliminación Fallida. No existe una fruta con el nombre {unaFruta.Nombre}.";
                    return false;
                }

                //Pasadas las validaciones, borramos la fruta
                try
                {
                    string eliminaFrutaSQL = "DELETE FROM frutas " +
                                             "WHERE id = @Id";

                    //Aqui no usamos parámetros dinámicos, pasamos el objeto!!!
                    cantidadFilas = cxnDB.Execute(eliminaFrutaSQL, unaFruta);
                    resultado = true;
                    mensajeEliminacion = "Eliminación exitosa!";

                }
                catch (NpgsqlException elError)
                {
                    resultado = false;
                    mensajeEliminacion = $"Error de borrado en la DB. {elError.Message}";
                }
            }

            return resultado;
        }                
        #endregion Frutas
    }
}
