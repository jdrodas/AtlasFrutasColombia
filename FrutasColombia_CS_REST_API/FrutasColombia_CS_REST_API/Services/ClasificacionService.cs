using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Services
{
    public class ClasificacionService(IClasificacionRepository clasificacionRepository)
    {
        private readonly IClasificacionRepository _clasificacionRepository = clasificacionRepository;

        public async Task<IEnumerable<Clasificacion>> GetAllAsync()
        {
            return await _clasificacionRepository
                .GetAllAsync();
        }
    }
}