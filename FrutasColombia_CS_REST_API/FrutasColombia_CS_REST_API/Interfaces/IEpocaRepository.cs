using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IEpocaRepository
    {
        public Task<IEnumerable<Epoca>> GetAllAsync();

        public Task<Epoca> GetByIdAsync(Guid epoca_id);

        public Task<Epoca> GetByNameAsync(string epoca_nombre);

        public Task<bool> CreateAsync(Epoca unaEpoca);
    }
}
