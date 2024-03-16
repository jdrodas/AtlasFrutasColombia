using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Interfaces
{
    public interface IFrutaRepository
    {
        public Task<IEnumerable<Fruta>> GetAllAsync();
    }
}