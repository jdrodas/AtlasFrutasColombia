using FrutasColombia_CS_NoSQL_REST_API.Models;

namespace FrutasColombia_CS_NoSQL_REST_API.Interfaces
{
    public interface IResumenRepository
    {
        public Task<Resumen> GetAllAsync();
    }
}
