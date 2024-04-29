/*
AppValidationException:
Excepcion creada para enviar mensajes relacionados 
con la validación en todas las operaciones CRUD de la aplicación
*/

namespace FrutasColombia_CS_REST_API.Exceptions
{
    public class AppValidationException(string message) : Exception(message)
    {
    }
}
