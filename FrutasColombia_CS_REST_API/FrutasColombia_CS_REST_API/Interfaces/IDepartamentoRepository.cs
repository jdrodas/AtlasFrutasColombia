using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IDepartamentoRepository
    {
        public Task<IEnumerable<Departamento>> GetAllAsync();

        public Task<Departamento> GetByIdAsync(Guid departamento_id);

        public Task<IEnumerable<Municipio>> GetAssociatedMunicipalityAsync(Guid departamento_id);
    }
}
