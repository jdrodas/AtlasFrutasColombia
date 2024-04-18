
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FrutasColombia_CS_PoC_Consola
{
    public class AccesoDatosMongo
    {
        public static string? ObtieneCadenaConexion()
        {
            //Parametrizamos el acceso al archivo de configuración appsettings.json
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration miConfiguracion = builder.Build();

            return miConfiguracion["ConnectionString:Mongo"];
        }

        public static List<Fruta> ObtieneListaFrutas()
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("frutas_db");
            var coleccionFrutas = "frutas";

            var listaFrutas = miDB.GetCollection<Fruta>(coleccionFrutas)
                .Find(new BsonDocument())
                .SortBy(fruta => fruta.Nombre)
                .ToList();

            return listaFrutas;
        }

        public static Fruta ObtieneFruta(string nombre_fruta)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("frutas_db");
            var coleccionFrutas = "frutas";

            var filtroFruta = new BsonDocument { { "nombre", nombre_fruta } };

            var unaFruta = miDB.GetCollection<Fruta>(coleccionFrutas)
                .Find(filtroFruta)
                .FirstOrDefault();

            return unaFruta;
        }

        public static List<string> ObtieneNombresFrutas()
        {
            var listaFrutas = ObtieneListaFrutas();

            List<string> listaNombresFrutas = [];

            foreach (Fruta unaFruta in listaFrutas)
                listaNombresFrutas.Add(unaFruta.Nombre!);

            return listaNombresFrutas;
        }

        public static string ObtenerObjectIdFruta(string nombre_fruta)
        {

            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("frutas_db");
            var coleccionFrutas = "frutas";

            var filtroFruta = new BsonDocument { { "nombre", nombre_fruta } };

            var unaFruta = miDB.GetCollection<Fruta>(coleccionFrutas)
                .Find(filtroFruta)
                .FirstOrDefault();

            return unaFruta.ObjectId!;
        }

        public static bool InsertaFruta(Fruta unaFruta)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("frutas_db");
            var miColeccion = miDB.GetCollection<Fruta>("frutas");

            miColeccion.InsertOne(unaFruta);

            string ObjectIdFruta = ObtenerObjectIdFruta(unaFruta.Nombre!);

            if (string.IsNullOrEmpty(ObjectIdFruta))
                return false;
            else
                return true;
        }

        public static bool ActualizaFruta(Fruta unaFruta)
        {
            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("frutas_db");
            var miColeccion = miDB.GetCollection<Fruta>("frutas");

            var resultadoActualizacion = miColeccion
                                            .ReplaceOne(fruta => fruta.ObjectId == unaFruta.ObjectId, unaFruta);

            return resultadoActualizacion.IsAcknowledged;
        }

        public static bool EliminaFruta(Fruta unaFruta, out string mensajeEliminacion)
        {
            mensajeEliminacion = string.Empty;

            string? cadenaConexion = ObtieneCadenaConexion();

            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase("frutas_db");
            var miColeccion = miDB.GetCollection<Fruta>("frutas");

            var resultadoEliminacion = miColeccion.DeleteOne(fruta => fruta.ObjectId == unaFruta.ObjectId);

            if (resultadoEliminacion.IsAcknowledged)
                mensajeEliminacion = "Fruta eliminada exitosamente!";
            else
                mensajeEliminacion = $"No se pudo borrar la fruta {unaFruta.Nombre!}";
            
            return resultadoEliminacion.IsAcknowledged;
        }
    }
}
