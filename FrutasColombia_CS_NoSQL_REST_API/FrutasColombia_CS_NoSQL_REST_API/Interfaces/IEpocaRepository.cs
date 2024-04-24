using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Interfaces
{
    public interface IEpocaRepository
    {
        public Task<IEnumerable<Epoca>> GetAllAsync();

        public Task<Epoca> GetByIdAsync(string epoca_id);

        public Task<Epoca> GetByNameAsync(string epoca_nombre);

        public Task<long> GetTotalByMonthRangeAsync(int mes_inicio, int mes_final);

        //public Task<int> GetTotalProductionById(Guid epoca_id);

        public Task<bool> CreateAsync(Epoca unaEpoca);

        public Task<bool> UpdateAsync(Epoca unaEpoca);

        public Task<bool> RemoveAsync(string epoca_id);
    }
}
