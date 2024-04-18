using FrutasColombia_CS_NoSQL_REST_API.Models;
using MongoDB.Driver;

namespace FrutasColombia_CS_NoSQL_REST_API.DbContexts
{
    public class MongoDbContext
    {
        private readonly string cadenaConexion;
        private readonly FrutasDatabaseSettings _frutasDatabaseSettings;
        public MongoDbContext(IConfiguration unaConfiguracion)
        {
            cadenaConexion = unaConfiguracion.GetConnectionString("Mongo")!;
            _frutasDatabaseSettings = new FrutasDatabaseSettings(unaConfiguracion);
        }

        public IMongoDatabase CreateConnection()
        {
            var clienteDB = new MongoClient(cadenaConexion);
            var miDB = clienteDB.GetDatabase(_frutasDatabaseSettings.DatabaseName);

            return miDB;
        }

        public FrutasDatabaseSettings configuracionColecciones
        {
            get { return _frutasDatabaseSettings; }
        }
    }
}