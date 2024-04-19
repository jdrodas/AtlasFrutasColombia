/*
DbOperationException:
Excepcion creada para enviar mensajes relacionados 
con problemas asociados a operaciones en base de datos
*/

namespace FrutasColombia_CS_REST_API.Exceptions
{
    public class DbOperationException : Exception
    {
        public DbOperationException(string message) : base(message) { }
    }
}