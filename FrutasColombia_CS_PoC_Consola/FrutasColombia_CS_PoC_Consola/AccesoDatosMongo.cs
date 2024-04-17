
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var ColeccionEstilos = "frutas";

            var listaFrutas = miDB.GetCollection<Fruta>(ColeccionEstilos)
                .Find(new BsonDocument())
                .SortBy(estilo => estilo.Nombre)
                .ToList();

            return listaFrutas;
        }

        public static List<string> ObtieneNombresFrutas()
        {
            var listaFrutas = ObtieneListaFrutas();

            List<string> listaNombresFrutas = [];

            foreach (Fruta unaFruta in listaFrutas)
                listaNombresFrutas.Add(unaFruta.Nombre!);

            return listaNombresFrutas;
        }
    }
}
