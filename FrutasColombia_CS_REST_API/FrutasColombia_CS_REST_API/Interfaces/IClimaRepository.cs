using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IClimaRepository
    {
        public Task<List<Clima>> GetAllAsync();

        public Task<Clima> GetByIdAsync(Guid clima_id);

        public Task<Clima> GetByNameAsync(string clima_nombre);

        public Task<Clima> GetByAltitudeRangeAsync(int altitud_minima, int altitud_maxima);

        public Task<int> GetTotalByAltitudeRangeAsync(int altitud_minima, int altitud_maxima);

        public Task<int> GetTotalProductionById(Guid clima_id);

        public Task<string> ValidateAltitudeRangeAsync(Clima unClima);

        public Task<bool> CreateAsync(Clima unClima);

        public Task<bool> UpdateAsync(Clima unClima);

        public Task<bool> RemoveAsync(Guid clima_id);
    }
}
