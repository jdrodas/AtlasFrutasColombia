using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IClasificacionRepository
    {
        public Task<IEnumerable<Clasificacion>> GetAllAsync();
    }
}
