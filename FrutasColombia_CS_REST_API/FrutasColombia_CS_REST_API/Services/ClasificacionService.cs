using FrutasColombia_CS_REST_API.Helpers;
using FrutasColombia_CS_REST_API.Interfaces;
using FrutasColombia_CS_REST_API.Models;

namespace FrutasColombia_CS_REST_API.Services
{
    public class ClasificacionService(IClasificacionRepository clasificacionRepository,
                                        IFrutaRepository frutaRepository)
    {
        private readonly IClasificacionRepository _clasificacionRepository = clasificacionRepository;
        private readonly IFrutaRepository _frutaRepository = frutaRepository;

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

        public async Task<IEnumerable<Genero>> GetAllGenusAsync()
        {
            return await _clasificacionRepository
                .GetAllGenusAsync();
        }

        public async Task<GeneroDetallado> GetGenusByIdAsync(int genero_id)
        {
            Genero unGenero = await _clasificacionRepository
                .GetGenusByIdAsync(genero_id);

            if (unGenero.Id == 0)
                throw new AppValidationException($"Género no encontrad0 con el id {genero_id}");

            GeneroDetallado unGeneroDetallado = await _clasificacionRepository
                .GetGenusDetailsByIdAsync(genero_id);

            return unGeneroDetallado;
        }

        public async Task<IEnumerable<FrutaClasificada>> GetFruitsByGenusIdAsync(int genero_id)
        {
            Genero unGenero = await _clasificacionRepository
                .GetGenusByIdAsync(genero_id);

            if (unGenero.Id == 0)
                throw new AppValidationException($"Género no encontrad0 con el id {genero_id}");

            var frutasClasificadas = await _frutaRepository
                .GetByGenusIdAsync(genero_id);

            if (!frutasClasificadas.Any())
                throw new AppValidationException($"Género {unGenero.Nombre} no tiene frutas producidas");

            return frutasClasificadas;
        }
    }
}