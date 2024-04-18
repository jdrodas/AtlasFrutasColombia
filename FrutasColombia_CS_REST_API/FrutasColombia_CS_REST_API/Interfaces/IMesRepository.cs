using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IMesRepository
    {
        public Task<IEnumerable<Mes>> GetAllAsync();

        public Task<Mes> GetByIdAsync(int mes_id);
    }
}
