using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Interfaces
{
    public interface IClimaRepository
    {
        public Task<IEnumerable<Clima>> GetAllAsync();

        public Task<Clima> GetByIdAsync(string clima_id);
    }
}
