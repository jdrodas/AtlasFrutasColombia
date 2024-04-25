using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Interfaces
{
    public interface IClimaRepository
    {
        public Task<IEnumerable<Clima>> GetAllAsync();

        public Task<Clima> GetByIdAsync(string clima_id);

        public Task<Clima> GetByNameAsync(string clima_nombre);

        public Task<long> GetTotalByAltitudeRangeAsync(int altitud_minima, int altitud_maxima);

        public Task<bool> CreateAsync(Clima unClima);

        public Task<bool> UpdateAsync(Clima unClima);

        public Task<bool> RemoveAsync(string epoca_id);
    }
}
