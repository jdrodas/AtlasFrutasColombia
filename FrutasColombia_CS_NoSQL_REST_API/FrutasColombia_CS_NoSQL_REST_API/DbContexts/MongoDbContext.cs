using FrutasColombia_CS_NoSQL_REST_API.Models;
using MongoDB.Driver;

namespace FrutasColombia_CS_NoSQL_REST_API.DbContexts
{
    public class MongoDbContext(IConfiguration unaConfiguracion)
    {
        private readonly string cadenaConexion = unaConfiguracion.GetConnectionString("Mongo")!;
        private readonly FrutasDatabaseSettings _frutasDatabaseSettings = new(unaConfiguracion);

        public IMongoDatabase CreateConnection()
        {
            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase(_frutasDatabaseSettings.DatabaseName);

            return miDB;
        }

        public FrutasDatabaseSettings ConfiguracionColecciones
        {
            get { return _frutasDatabaseSettings; }
        }
    }
}