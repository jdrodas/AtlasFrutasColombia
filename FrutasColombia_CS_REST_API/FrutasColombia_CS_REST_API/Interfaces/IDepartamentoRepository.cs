using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IDepartamentoRepository
    {
        public Task<IEnumerable<Departamento>> GetAllAsync();

        public Task<Departamento> GetByIdAsync(string departamento_id);

        public Task<IEnumerable<Municipio>> GetAssociatedMunicipiosAsync(string departamento_id);
    }
}
