using FrutasColombia_CS_REST_API.Helpers;
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

        public async Task<IEnumerable<Division>> GetAllDivisionsAsync()
        {
            return await _clasificacionRepository
                .GetAllDivisionsAsync();
        }

        public async Task<IEnumerable<Clase>> GetAllClassesAsync()
        {
            return await _clasificacionRepository
                .GetAllClassesAsync();
        }

        public async Task<IEnumerable<Orden>> GetAllOrdersAsync()
        {
            return await _clasificacionRepository
                .GetAllOrdersAsync();
        }

        public async Task<IEnumerable<Familia>> GetAllFamiliesAsync()
        {
            return await _clasificacionRepository
                .GetAllFamiliesAsync();
        }
    }
}