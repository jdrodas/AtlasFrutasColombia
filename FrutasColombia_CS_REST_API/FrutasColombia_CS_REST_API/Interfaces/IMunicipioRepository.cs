using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IMunicipioRepository
    {
        public Task<List<Municipio>> GetAllAsync();

        public Task<Municipio> GetByIdAsync(Guid municipio_id);
    }
}