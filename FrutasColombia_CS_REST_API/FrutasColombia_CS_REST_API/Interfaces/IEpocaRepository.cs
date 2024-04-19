using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IEpocaRepository
    {
        public Task<IEnumerable<Epoca>> GetAllAsync();

        public Task<Epoca> GetByIdAsync(Guid epoca_id);
    }
}
