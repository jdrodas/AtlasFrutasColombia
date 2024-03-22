using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IFrutaRepository
    {
        public Task<IEnumerable<Fruta>> GetAllAsync();

        public Task<Fruta> GetByIdAsync(int fruta_id);

        public Task<Fruta> GetByNameAsync(string fruta_nombre);

        public Task<bool> CreateAsync(Fruta unaFruta);

        public Task<bool> UpdateAsync(Fruta unaFruta);

        public Task<bool> RemoveAsync(int fruta_id);
    }
}